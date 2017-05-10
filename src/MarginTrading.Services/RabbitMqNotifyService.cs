﻿using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Common;
using Common.Log;
using MarginTrading.Common.Mappers;
using MarginTrading.Core;
using MarginTrading.Core.Settings;

namespace MarginTrading.Services
{
    public class RabbitMqNotifyService : IRabbitMqNotifyService
    {
        private readonly MarginSettings _settings;
        private readonly IIndex<string, IMessageProducer<string>> _publishers;
        private readonly ILog _log;

        public RabbitMqNotifyService(
            MarginSettings settings,
            IIndex<string, IMessageProducer<string>> publishers,
            ILog log)
        {
            _settings = settings;
            _publishers = publishers;
            _log = log;
        }
        public async Task AccountHistory(string accountId, string clientId, double amount, double balance, AccountHistoryType type, string comment = null)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.AccountHistory.RoutingKeyName].ProduceAsync(new MarginTradingAccountHistory
                {
                    Id = Guid.NewGuid().ToString("N"),
                    AccountId = accountId,
                    ClientId = clientId,
                    Type = type,
                    Amount = amount,
                    Balance = balance,
                    Date = DateTime.UtcNow,
                    Comment = comment
                }.ToBackendContract().ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(AccountHistory),
                    $"accountId: {accountId}, clientId: {clientId}, amount: {amount}, balance: {balance}, type: {type}, comment: {comment}",
                    ex);
            }
        }

        public async Task OrderHistory(IOrder order)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.OrderHistory.RoutingKeyName].ProduceAsync(order.ToFullContract().ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(OrderHistory), $"orderId: {order.Id}, accountId: {order.AccountId}, clientId: {order.ClientId}",
                    ex);
            }
        }

        public async Task OrdeReject(IOrder order)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.OrderRejected.RoutingKeyName].ProduceAsync(order.ToFullContract().ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(OrdeReject),
                    $"orderId: {order.Id}, accountId: {order.AccountId}, clientId: {order.ClientId}",
                    ex);
            }
        }

        public async Task OrderBookPrice(InstrumentBidAskPair quote)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.OrderbookPrices.RoutingKeyName].ProduceAsync(quote.ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(OrderBookPrice),
                    $"instrument: {quote.Instrument}, bid: {quote.Bid}, ask: {quote.Ask}, data: {quote.Date:u}",
                    ex);
            }
        }

        public async Task OrderChanged(IOrder order)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.OrderChanged.RoutingKeyName].ProduceAsync(order.ToBaseContract().ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(OrderChanged),
                    $"orderId: {order.Id}, accountId: {order.AccountId}, clientId: {order.ClientId}",
                    ex);
            }
        }

        public async Task AccountChanged(IMarginTradingAccount account)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.AccountChanged.RoutingKeyName].ProduceAsync(account.ToBackendContract().ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(AccountChanged),
                    $"accountId: {account.Id}, clientId: {account.ClientId}",
                    ex);
            }
        }

        public async Task AccountStopout(string clientId, string accountId, int positionsCount, double totalPnl)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.AccountStopout.RoutingKeyName].ProduceAsync(new {clientId = clientId, accountId = accountId, positionsCount = positionsCount, totalPnl = totalPnl}.ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(AccountStopout),
                    $"accountId: {accountId}, positions count: {positionsCount}, total PnL: {totalPnl}", ex);
            }
        }

        public async Task UserUpdates(bool updateAccountAssets, bool updateAccounts, string[] clientIds)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.UserUpdates.RoutingKeyName].ProduceAsync(new { updateAccountAssetPairs = updateAccountAssets, UpdateAccounts = updateAccounts, clientIds = clientIds}.ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(UserUpdates), null, ex);
            }
        }

        public async Task TransactionCreated(ITransaction transaction)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.Transaction.RoutingKeyName].ProduceAsync(transaction.ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(TransactionCreated), $"takerOrderId: {transaction.TakerOrderId}, takerAccountId: {transaction.TakerAccountId}, takerId: {transaction.TakerLykkeId}, makerOrderId: {transaction.MakerOrderId}, makerId: {transaction.MakerLykkeId}", ex);
            }
        }

        public async Task OrderActionCreated(IOrderAction orderAction)
        {
            try
            {
                await _publishers[_settings.RabbitMqQueues.OrderReport.RoutingKeyName].ProduceAsync(orderAction.ToJson());
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(RabbitMqNotifyService), nameof(OrderActionCreated), $"orderId: {orderAction.LykkeOrderId}, traderId: {orderAction.TraderLykkeId}", ex);
            }
        }

        public void Stop()
        {
            ((IStopable)_publishers[_settings.RabbitMqQueues.AccountHistory.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.OrderHistory.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.OrderRejected.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.OrderbookPrices.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.AccountStopout.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.AccountChanged.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.UserUpdates.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.Transaction.RoutingKeyName]).Stop();
            ((IStopable)_publishers[_settings.RabbitMqQueues.OrderReport.RoutingKeyName]).Stop();
        }
    }
}
