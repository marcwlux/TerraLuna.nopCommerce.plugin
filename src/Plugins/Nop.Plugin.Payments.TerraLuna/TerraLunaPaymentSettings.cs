﻿using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.TerraLuna
{
    /// <summary>
    /// Represents settings of manual payment plugin
    /// </summary>
    public class TerraLunaPaymentSettings : ISettings
    {
        /// <summary>
        /// Gets or sets payment transaction mode
        /// </summary>
        public TransactMode TransactMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }

        /// <summary>
        /// Gets or sets an additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }

        /// <summary>
        /// Gets or sets the TatumApiKey
        /// </summary>
        public string TatumApiKey { get; set; }

        /// <summary>
        /// Gets or sets the BitcoinAddress
        /// </summary>
        public string BitcoinAddress { get; set; }
    }
}
