﻿using System;
using System.Collections.Generic;
using MarginTrading.Backend.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace MarginTrading.AzureRepositories.Entities
{
	public class OvernightSwapStateEntity : TableEntity, IOvernightSwapState
	{
		public string ClientId { get; set; }
		public string AccountId { get; set; }
		public string Instrument { get; set; }
		public string Direction { get; set; }
		OrderDirection? IOvernightSwapState.Direction => 
			Enum.TryParse<OrderDirection>(Direction, out var direction) ? direction : (OrderDirection?)null;
		public DateTime Time { get; set; }
		public double Volume { get; set; }
		decimal IOvernightSwapState.Volume => (decimal) Volume;
		public string OpenOrderId { get; set; }
		string IOvernightSwapState.OpenOrderId => OpenOrderId;
		public double Value { get; set; }
		decimal IOvernightSwapState.Value => (decimal) Value;
		public double SwapRate { get; set; }
		decimal IOvernightSwapState.SwapRate => (decimal) SwapRate;
		
		public static OvernightSwapStateEntity Create(IOvernightSwapState obj)
		{
			return new OvernightSwapStateEntity
			{
				PartitionKey = obj.AccountId,
				RowKey = obj.OpenOrderId,
				ClientId = obj.ClientId,
				AccountId = obj.AccountId,
				Instrument = obj.Instrument,
				Direction = obj.Direction?.ToString(),
				Time = obj.Time,
				Volume = (double) obj.Volume,
				Value = (double) obj.Value,
				SwapRate = (double) obj.SwapRate,
				OpenOrderId = obj.OpenOrderId,
			};
		}
	}
}