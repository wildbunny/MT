﻿namespace MarginTrading.Common.ClientContracts
{
    public class MtClientResponse<T>
    {
        public T Result { get; set; }
        public string Message { get; set; }
    }
}
