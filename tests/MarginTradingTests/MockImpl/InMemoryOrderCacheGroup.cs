﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarginTrading.Backend.Core;
using MarginTrading.Backend.Core.Messages;
using StackExchange.Redis;
using Order = MarginTrading.Backend.Core.Order;

namespace MarginTradingTests.MockImpl
{
    public class InMemoryOrderCacheGroup : IOrderCacheGroup
    {
        private Dictionary<string, Order> _ordersById;
        private Dictionary<string, HashSet<string>> _orderIdsByAccountId;
        private Dictionary<string, HashSet<string>> _orderIdsByInstrumentId;
        private Dictionary<(string, string), HashSet<string>> _orderIdsByAccountIdAndInstrumentId;
        private Dictionary<string, HashSet<string>> _orderIdsByMarginInstrumentId;
        
        public OrderStatus Status { get; }
        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();

        public InMemoryOrderCacheGroup(OrderStatus orderStatus)
        {
            Status = orderStatus;
        }

        public IOrderCacheGroup Init(IReadOnlyCollection<Order> orders)
        {
            var statusOrders = orders.Where(x => x.Status == Status).ToList();

            _lockSlim.EnterWriteLock();

            try
            {
                _ordersById = statusOrders.ToDictionary(x => x.Id);

                _orderIdsByInstrumentId = statusOrders.GroupBy(x => x.Instrument)
                    .ToDictionary(x => x.Key, x => x.Select(o => o.Id).ToHashSet());

                _orderIdsByAccountId = statusOrders.GroupBy(x => x.AccountId)
                    .ToDictionary(x => x.Key, x => x.Select(o => o.Id).ToHashSet());

                _orderIdsByAccountIdAndInstrumentId = statusOrders.GroupBy(x => GetAccountInstrumentCacheKey(x.AccountId, x.Instrument))
                    .ToDictionary(x => x.Key, x => x.Select(o => o.Id).ToHashSet());

                _orderIdsByMarginInstrumentId = statusOrders.Where(x => !string.IsNullOrEmpty(x.MarginCalcInstrument))
                    .GroupBy(x => x.MarginCalcInstrument)
                    .ToDictionary(x => x.Key, x => x.Select(o => o.Id).ToHashSet());
            }
            finally 
            {
                _lockSlim.ExitWriteLock();
            }

            return this;
        }


        #region Setters

        public Task AddAsync(Order order)
        {
            _lockSlim.EnterWriteLock();

            try
            {
                _ordersById.Add(order.Id, order);

                if (!_orderIdsByAccountId.ContainsKey(order.AccountId))
                    _orderIdsByAccountId.Add(order.AccountId, new HashSet<string>());
                _orderIdsByAccountId[order.AccountId].Add(order.Id);

                if (!_orderIdsByInstrumentId.ContainsKey(order.Instrument))
                    _orderIdsByInstrumentId.Add(order.Instrument, new HashSet<string>());
                _orderIdsByInstrumentId[order.Instrument].Add(order.Id);

                var accountInstrumentCacheKey = GetAccountInstrumentCacheKey(order.AccountId, order.Instrument);

                if (!_orderIdsByAccountIdAndInstrumentId.ContainsKey(accountInstrumentCacheKey))
                    _orderIdsByAccountIdAndInstrumentId.Add(accountInstrumentCacheKey, new HashSet<string>());
                _orderIdsByAccountIdAndInstrumentId[accountInstrumentCacheKey].Add(order.Id);

                if (!string.IsNullOrEmpty(order.MarginCalcInstrument))
                {
                    if(!_orderIdsByMarginInstrumentId.ContainsKey(order.MarginCalcInstrument))
                        _orderIdsByMarginInstrumentId.Add(order.MarginCalcInstrument, new HashSet<string>());
                    _orderIdsByMarginInstrumentId[order.MarginCalcInstrument].Add(order.Id);
                }
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }

            var account = MtServiceLocator.AccountsCacheService.Get(order.ClientId, order.AccountId);
            account.CacheNeedsToBeUpdated();

            return Task.CompletedTask;
        }

        public Task RemoveAsync(Order order)
        {
            _lockSlim.EnterWriteLock();

            try
            {
                if (_ordersById.Remove(order.Id))
                {
                    _orderIdsByInstrumentId[order.Instrument].Remove(order.Id);
                    _orderIdsByAccountId[order.AccountId].Remove(order.Id);
                    _orderIdsByAccountIdAndInstrumentId[GetAccountInstrumentCacheKey(order.AccountId, order.Instrument)].Remove(order.Id);

                    if (!string.IsNullOrEmpty(order.MarginCalcInstrument)
                        && (_orderIdsByMarginInstrumentId[order.MarginCalcInstrument]?.Contains(order.Id) ?? false))
                        _orderIdsByMarginInstrumentId[order.MarginCalcInstrument].Remove(order.Id);
                }
                else
                    throw new Exception(string.Format(MtMessages.CantRemoveOrderWithStatus, order.Id, Status));
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }

            var account = MtServiceLocator.AccountsCacheService?.Get(order.ClientId, order.AccountId);
            account?.CacheNeedsToBeUpdated();

            return Task.CompletedTask;
        }

        #endregion


        #region Getters

        public Order GetOrderById(string orderId)
        {
            if (TryGetOrderById(orderId, out var result))
                return result;

            throw new Exception(string.Format(MtMessages.CantGetOrderWithStatus, orderId, Status));
        }

        public bool TryGetOrderById(string orderId, out Order result)
        {
            _lockSlim.EnterReadLock();

            try
            {
                if (!_ordersById.ContainsKey(orderId))
                {
                    result = null;
                    return false;
                }
                result = _ordersById[orderId];
                return true;
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        public IReadOnlyCollection<Order> GetOrdersByInstrument(string instrument)
        {
            if (string.IsNullOrWhiteSpace(instrument))
                throw new ArgumentException(nameof(instrument));

            _lockSlim.EnterReadLock();

            try
            {
                if (!_orderIdsByInstrumentId.ContainsKey(instrument))
                    return new List<Order>();

                return _orderIdsByInstrumentId[instrument].Select(id => _ordersById[id]).ToList();
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        public IReadOnlyCollection<Order> GetOrdersByMarginInstrument(string instrument)
        {
            if (string.IsNullOrWhiteSpace(instrument))
                throw new ArgumentException(nameof(instrument));

            _lockSlim.EnterReadLock();

            try
            {
                if (!_orderIdsByMarginInstrumentId.ContainsKey(instrument))
                    return new List<Order>();

                return _orderIdsByMarginInstrumentId[instrument].Select(id => _ordersById[id]).ToList();
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        public ICollection<Order> GetOrdersByInstrumentAndAccount(string instrument, string accountId)
        {
            if (string.IsNullOrWhiteSpace(instrument))
                throw new ArgumentException(nameof(instrument));

            if (string.IsNullOrWhiteSpace(accountId))
                throw new ArgumentException(nameof(instrument));

            var key = GetAccountInstrumentCacheKey(accountId, instrument);

            _lockSlim.EnterReadLock();

            try
            {
                if (!_orderIdsByAccountIdAndInstrumentId.ContainsKey(key))
                    return new List<Order>();

                return _orderIdsByAccountIdAndInstrumentId[key].Select(id => _ordersById[id]).ToList();
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        public IReadOnlyCollection<Order> GetAllOrders()
        {
            _lockSlim.EnterReadLock();

            try
            {
                return _ordersById.Values;
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        public ICollection<Order> GetOrdersByAccountIds(params string[] accountIds)
        {
            _lockSlim.EnterReadLock();

            var result = new List<Order>();

            try
            {
                foreach (var accountId in accountIds)
                {
                    if (!_orderIdsByAccountId.ContainsKey(accountId))
                        continue;

                    foreach (var orderId in _orderIdsByAccountId[accountId])
                        result.Add(_ordersById[orderId]);
                }

                return result;
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }
        #endregion


        #region Helpers

        private (string, string) GetAccountInstrumentCacheKey(string accountId, string instrumentId)
        {
            return (accountId, instrumentId);
        }

        #endregion
    }
}