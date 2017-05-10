﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarginTrading.Backend.Models;
using MarginTrading.Common.Models;
using MarginTrading.Core;
using MarginTrading.Core.Clients;
using MarginTrading.Services;
using MarginTrading.Services.Generated.ClientAccountServiceApi;
using MarginTrading.Services.Generated.ClientAccountServiceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MarginTrading.Backend.Controllers
{
    [Authorize]
    [Route("api/backoffice")]
    public class BackOfficeController : Controller
    {
        private readonly ITradingConditionsCacheService _tradingConditionsCacheService;
        private readonly IAccountGroupCacheService _accountGroupCacheService;
        private readonly AccountAssetsCacheService _accountAssetsCacheService;
        private readonly IInstrumentsCache _instrumentsCache;
        private readonly IAccountsCacheService _accountsCacheService;
        private readonly IMatchingEngineRoutesCacheService _routesCacheService;
        private readonly AccountManager _accountManager;
        private readonly TradingConditionsManager _tradingConditionsManager;
        private readonly AccountGroupManager _accountGroupManager;
        private readonly AccountAssetsManager _accountAssetsManager;
        private readonly MatchingEngineRoutesManager _routesManager;
        private readonly IMarginTradingAccountsRepository _accountsRepository;
        private readonly IClientAccountService _clientAccountService;
        private readonly IOrderReader _ordersReader;
        private readonly OrderBookList _orderBooks;

        public BackOfficeController(
            ITradingConditionsCacheService tradingConditionsCacheService,
            IAccountGroupCacheService accountGroupCacheService,
            AccountAssetsCacheService accountAssetsCacheService,
            IInstrumentsCache instrumentsCache,
            IAccountsCacheService accountsCacheService,
            IMatchingEngineRoutesCacheService routesCacheService,
            AccountManager accountCacheManager,
            TradingConditionsManager tradingConditionsManager,
            AccountGroupManager accountGroupManager,
            AccountAssetsManager accountAssetsManager,
            MatchingEngineRoutesManager routesManager,
            IMarginTradingAccountsRepository accountsRepository,
            IClientAccountService clientAccountService,
            IOrderReader ordersReader,
            OrderBookList orderBooks)
        {
            _tradingConditionsCacheService = tradingConditionsCacheService;
            _accountGroupCacheService = accountGroupCacheService;
            _accountAssetsCacheService = accountAssetsCacheService;
            _instrumentsCache = instrumentsCache;
            _accountsCacheService = accountsCacheService;
            _routesCacheService = routesCacheService;

            _accountManager = accountCacheManager;
            _tradingConditionsManager = tradingConditionsManager;
            _accountGroupManager = accountGroupManager;
            _accountAssetsManager = accountAssetsManager;
            _routesManager = routesManager;
            _accountsRepository = accountsRepository;
            _clientAccountService = clientAccountService;
            _ordersReader = ordersReader;
            _orderBooks = orderBooks;
        }

        /// <summary>
        /// Returns summary asset info 
        /// </summary>
        /// <remarks>
        /// VolumeLong is a sum of long positions volume
        /// 
        /// VolumeShort is a sum of short positions volume
        /// 
        /// PnL is a sum of all positions PnL
        /// 
        /// Header "api-key" is required
        /// </remarks>
        /// <response code="200">Returns summary info by assets</response>
        [HttpGet]
        [Route("assetsInfo")]
        [ProducesResponseType(typeof(List<SummaryAssetInfo>), 200)]
        public List<SummaryAssetInfo> GetAssetsInfo()
        {
            var result = new List<SummaryAssetInfo>();
            IEnumerable<Order> orders = _ordersReader.GetAll().ToList();

            foreach (var order in orders)
            {
                var assetInfo = result.FirstOrDefault(item => item.AssetPairId == order.Instrument);

                if (assetInfo == null)
                {
                    result.Add(new SummaryAssetInfo
                    {
                        AssetPairId = order.Instrument,
                        PnL = order.GetFpl(),
                        VolumeLong = order.GetOrderType() == OrderDirection.Buy ? order.GetMatchedVolume() : 0,
                        VolumeShort = order.GetOrderType() == OrderDirection.Sell ? order.GetMatchedVolume() : 0
                    });
                }
                else
                {
                    assetInfo.PnL += order.GetFpl();

                    if (order.GetOrderType() == OrderDirection.Buy)
                    {
                        assetInfo.VolumeLong += order.GetMatchedVolume();
                    }
                    else
                    {
                        assetInfo.VolumeShort += order.GetMatchedVolume();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns list of opened positions
        /// </summary>
        /// <remarks>
        /// Returns list of opened positions with matched volume greater or equal provided "volume" parameter
        /// 
        /// Header "api-key" is required
        /// </remarks>
        /// <response code="200">Returns opened positions</response>
        [HttpGet]
        [Route("positionsByVolume")]
        [ProducesResponseType(typeof(List<Order>), 200)]
        public List<Order> GetPositionsByVolume([FromQuery]double volume)
        {
            var result = new List<Order>();
            IEnumerable<Order> orders = _ordersReader.GetAll();

            foreach (var order in orders.Where(item => item.Status == OrderStatus.Active))
            {
                if (order.GetMatchedVolume() >= volume)
                {
                    result.Add(order);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns list of pending orders
        /// </summary>
        /// <remarks>
        /// Returns list of pending orders with volume greater or equal provided "volume" parameter
        /// 
        /// Header "api-key" is required
        /// </remarks>
        /// <response code="200">Returns pending orders</response>
        [HttpGet]
        [Route("pendingOrdersByVolume")]
        [ProducesResponseType(typeof(List<Order>), 200)]
        public List<Order> GetPendingOrdersByVolume([FromQuery]double volume)
        {
            var result = new List<Order>();
            IEnumerable<Order> orders = _ordersReader.GetAll();

            foreach (var order in orders.Where(item => item.Status == OrderStatus.WaitingForExecution))
            {
                if (Math.Abs(order.Volume) >= volume)
                {
                    result.Add(order);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns list of orderbooks
        /// </summary>
        /// <remarks>
        /// Returns list of orderbooks by instrument (all orderbooks if no instrument is provided)
        /// 
        /// Header "api-key" is required
        /// </remarks>
        /// <response code="200">Returns orderbooks</response>
        [HttpGet]
        [Route("orderbooks")]
        [ProducesResponseType(typeof(List<OrderBookModel>), 200)]
        public List<OrderBookModel> GetOrderBooks([FromQuery] string instrument = null)
        {
            // TODO: return aggregated order book
            var result = new List<OrderBookModel>();

            var orderbooks = _orderBooks.GetAllLimitOrders(instrument);

            result.Add(
                new OrderBookModel
                {
                    Instrument = instrument,
                    Buy = orderbooks.Buy.ToList(),
                    Sell = orderbooks.Sell.ToList()
                });

            /*
            foreach (var orderbook in orderbooks)
            {
                var model = new OrderBookModel
                {
                    Instrument = orderbook.Instrument
                };

                foreach (var orders in orderbook.Buy.Values)
                {
                    model.Buy.AddRange(orders);
                }

                foreach (var orders in orderbook.Sell.Values)
                {
                    model.Sell.AddRange(orders);
                }

                model.Buy = model.Buy.OrderByDescending(item => item.Price).ToList();
                model.Sell = model.Sell.OrderBy(item => item.Price).ToList();

                result.Add(model);
            }
            */

            return result;
        }

        /// <summary>
        /// Updates trading conditions
        /// </summary>
        /// <remarks>
        /// Call this method to update trading conditions
        /// 
        /// Header "api-key" is required
        /// </remarks>
        [HttpPost]
        [Route("updateTradingConditions")]
        public async Task<IActionResult> UpdateTradingConditions([FromQuery]string tradingConditionId)
        {
            await _tradingConditionsManager.UpdateTradingConditions(tradingConditionId);
            await _accountGroupManager.UpdateAccountGroupCache();
            await _accountAssetsManager.UpdateAccountAssetsCache();
            return Ok();
        }


        /// <summary>
        /// Updates accounts cache
        /// </summary>
        /// <remarks>
        /// Call this method to update accounts cache
        /// 
        /// Header "api-key" is required
        /// </remarks>
        [HttpPost]
        [Route("updateAccounts")]
        public async Task<IActionResult> UpdateAccountsCache([FromQuery]string clientId)
        {
            await _accountManager.UpdateAccountsCacheAsync(clientId);

            return Ok();
        }

        /// <summary>
        /// Creates margin trading account
        /// </summary>
        /// <remarks>
        /// Header "api-key" is required
        /// </remarks>
        [HttpPost]
        [Route("createMarginTradingAccount")]
        [ProducesResponseType(typeof(MtResponse<string>), 200)]
        [ProducesResponseType(typeof(MtResponse<string>), 400)]
        public async Task<IActionResult> CreateMarginTradingAccount([FromBody]CreateMarginTradingAccountModel model)
        {
            var result = new MtResponse<string>();

            if (ModelState.IsValid)
            {
                IClientAccount client = await _clientAccountService.ApiClientAccountsGetByIdPostAsync(new GetByIdRequest(model.ClientId));

                if (client == null)
                {
                    result.Message = "Client not found";
                    return BadRequest(result);
                }

                var tradingCondition = _tradingConditionsCacheService.GetTradingCondition(model.TradingConditionId);

                if (tradingCondition == null)
                {
                    result.Message = "Trading condition not found";
                    return BadRequest(result);
                }

                string accountId = Guid.NewGuid().ToString("N");

                var account = new MarginTradingAccount
                {
                    Id = accountId,
                    BaseAssetId = model.AssetId,
                    ClientId = model.ClientId,
                    TradingConditionId = model.TradingConditionId
                };

                await _accountsRepository.AddAsync(account);
                _accountsCacheService.AddAccount(account);
                result.Result = accountId;
                return Ok(result);
            }

            ModelErrorCollection errors = ModelState.Values.Select(v => v.Errors).FirstOrDefault();

            string error = "Unknown error";

            if (errors != null)
            {
                error = errors[0].ErrorMessage;
            }
            result.Message = error;

            return BadRequest(result);
        }

        /// <summary>
        /// Sets trading condition for account
        /// </summary>
        /// <remarks>
        /// 
        /// Header "api-key" is required
        /// </remarks>
        /// <response code="200">Returns true if trading condition is set</response>
        [HttpPost]
        [Route("setTradingCondition")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> SetTradingCondition([FromBody]SetTradingConditionModel model)
        {
            bool result;

            var tradingCondition = _tradingConditionsCacheService.GetTradingCondition(model.TradingConditionId);

            if (tradingCondition == null)
            {
                throw new Exception($"No trading condition {model.TradingConditionId} found in cache");
            }

            _accountsCacheService.SetTradingCondition(model.ClientId, model.AccountId, model.TradingConditionId);

            result = await _accountsRepository.UpdateTradingConditionIdAsync(model.AccountId, model.TradingConditionId);

            if (result)
            {
                await _tradingConditionsManager.UpdateTradingConditions(model.TradingConditionId, model.AccountId);
            }

            return Ok(result);
        }

        #region Trading conditions

        [HttpGet]
        [Route("tradingConditions/getall")]
        [ProducesResponseType(typeof(List<MarginTradingCondition>), 200)]
        public IActionResult GetAllTradingConditions()
        {
            var tradingConditions = _tradingConditionsCacheService.GetAllTradingConditions();
            return Ok(tradingConditions);
        }

        [HttpGet]
        [Route("tradingConditions/get/{id}")]
        [ProducesResponseType(typeof(MarginTradingCondition), 200)]
        public IActionResult GetTradingCondition(string id)
        {
            var tradingCondition = _tradingConditionsCacheService.GetTradingCondition(id);
            return Ok(tradingCondition);
        }

        [HttpPost]
        [Route("tradingConditions/add")]
        public async Task<IActionResult> AddOrReplaceTradingCondition([FromBody]MarginTradingCondition model)
        {
            if (_tradingConditionsCacheService.GetTradingCondition(model.Id) == null)
            {
                await _accountGroupManager.AddAccountGroupsForTradingCondition(model.Id);
            }

            await _tradingConditionsManager.AddOrReplaceTradingConditionAsync(model);

            return Ok();
        }

        #endregion
        
        #region Account groups

        [HttpGet]
        [Route("accountGroups/getall")]
        [ProducesResponseType(typeof(List<MarginTradingAccountGroup>), 200)]
        public IActionResult GetAllAccountGrpups()
        {
            var accountGrpups = _accountGroupCacheService.GetAllAccountGroups();
            return Ok(accountGrpups);
        }

        [HttpGet]
        [Route("accountGroups/get/{tradingConditionId}/{id}")]
        [ProducesResponseType(typeof(MarginTradingAccountGroup), 200)]
        public IActionResult GetAccountGroup(string tradingConditionId, string id)
        {
            var accountGroup = _accountGroupCacheService.GetAccountGroup(tradingConditionId, id);
            return Ok(accountGroup);
        }

        [HttpPost]
        [Route("accountGroups/add")]
        public async Task<IActionResult> AddOrReplaceAccountGroup([FromBody]MarginTradingAccountGroup model)
        {
            await _accountGroupManager.AddOrReplaceAccountGroupAsync(model);
            await _tradingConditionsManager.UpdateTradingConditions(model.TradingConditionId);

            return Ok();
        }

        #endregion

        #region Account assets

        [HttpGet]
        [Route("accountAssets/getall/{tradingConditionId}/{accountAssetId}")]
        [ProducesResponseType(typeof(List<MarginTradingAccountAsset>), 200)]
        public IActionResult GetAllAccountAssets(string tradingConditionId, string accountAssetId)
        {
            var accountAssets = _accountAssetsCacheService.GetAccountAssets(tradingConditionId, accountAssetId);
            return Ok(accountAssets);
        }

        [HttpGet]
        [Route("accountAssets/get/{tradingConditionId}/{baseAssetId}/{instrumet}")]
        [ProducesResponseType(typeof(MarginTradingAccountAsset), 200)]
        public IActionResult GetAccountAssets(string tradingConditionId, string baseAssetId, string instrumet)
        {
            var accountAsset = _accountAssetsCacheService.GetAccountAssetNoThrowExceptionOnInvalidData(tradingConditionId, baseAssetId, instrumet);
            return Ok(accountAsset);
        }

        [HttpPost]
        [Route("accountAssets/assignInstruments")]
        [ProducesResponseType(typeof(MarginTradingAccountAsset), 200)]
        public async Task<IActionResult> AssignInstruments([FromBody]AssignInstrumentsModel model)
        {
            await _accountAssetsManager.AssignInstruments(model.TradingConditionId, model.BaseAssetId, model.Instruments);
            await _tradingConditionsManager.UpdateTradingConditions(model.TradingConditionId);

            return Ok();
        }

        [HttpPost]
        [Route("accountAssets/add")]
        public async Task<IActionResult> AddOrReplaceAccountAsset([FromBody]MarginTradingAccountAsset model)
        {
            await _accountAssetsManager.AddOrReplaceAccountAssetAsync(model);
            await _tradingConditionsManager.UpdateTradingConditions(model.TradingConditionId);

            return Ok();
        }

        #endregion

        [HttpGet]
        [Route("instruments/getall")]
        [ProducesResponseType(typeof(List<MarginTradingAsset>), 200)]
        public IActionResult GetAllInstruments()
        {
            var instruments = _instrumentsCache.GetAll();
            return Ok(instruments);
        }

        [HttpGet]
        [Route("matchingengines/getall")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public IActionResult GetAllMatchingEngines()
        {
            var matchingEngines = MatchingEngines.All;
            return Ok(matchingEngines);
        }

        [HttpGet]
        [Route("orderTypes/getall")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public IActionResult GetAllOrderTypes()
        {
            var orderTypes = Enum.GetNames(typeof(OrderDirection));
            return Ok(orderTypes);
        }

        #region Accounts

        [HttpGet]
        [Route("marginTradingAccounts/getall/{clientId}")]
        [ProducesResponseType(typeof(List<MarginTradingAccount>), 200)]
        public IActionResult GetAllMarginTradingAccounts(string clientId)
        {
            var accounts = _accountsCacheService.GetAll(clientId);
            return Ok(accounts);
        }

        [HttpPost]
        [Route("marginTradingAccounts/delete/{clientId}/{accountId}")]
        public async Task<IActionResult> DeleteMarginTradingAccount(string clientId, string accountId)
        {
            await _accountManager.DeleteAccountAsync(clientId, accountId);
            return Ok();
        }

        [HttpPost]
        [Route("marginTradingAccounts/add")]
        public async Task<IActionResult> AddMarginTradingAccount([FromBody]MarginTradingAccount account)
        {
            await _accountManager.AddAccountAsync(account);
            return Ok();
        }

        #endregion

        #region Matching engine routes

        [HttpGet]
        [Route("routes/getallglobal")]
        [ProducesResponseType(typeof(List<MatchingEngineRoute>), 200)]
        public IActionResult GetAllGlobalRoutes()
        {
            var routes = _routesCacheService.GetGlobalRoutes();
            return Ok(routes);
        }

        [HttpGet]
        [Route("routes/getalllocal")]
        [ProducesResponseType(typeof(List<MatchingEngineRoute>), 200)]
        public IActionResult GetAllLocalRoutes()
        {
            var routes = _routesCacheService.GetLocalRoutes();
            return Ok(routes);
        }

        [HttpGet]
        [Route("routes/get/{id}")]
        [ProducesResponseType(typeof(MatchingEngineRoute), 200)]
        public IActionResult GetRoute(string id)
        {
            var route = _routesCacheService.GetMatchingEngineRouteById(id);
            return Ok(route);
        }

        [HttpPost]
        [Route("routes/addglobal")]
        public async Task<IActionResult> AddGlobalRoute([FromBody]MatchingEngineRoute route)
        {
            await _routesManager.AddOrReplaceGlobalRouteAsync(route);
            return Ok();
        }

        [HttpPost]
        [Route("routes/addlocal")]
        public async Task<IActionResult> AddLocalRoute([FromBody]MatchingEngineRoute route)
        {
            await _routesManager.AddOrReplaceLocalRouteAsync(route);
            return Ok();
        }

        [HttpPost]
        [Route("routes/deleteglobal/{id}")]
        public async Task<IActionResult> DeleteGlobalRoute(string id)
        {
            await _routesManager.DeleteGlobalRouteAsync(id);
            return Ok();
        }

        [HttpPost]
        [Route("routes/deletelocal/{id}")]
        public async Task<IActionResult> DeleteLocalRoute(string id)
        {
            await _routesManager.DeleteLocalRouteAsync(id);
            return Ok();
        }

        #endregion
    }
}