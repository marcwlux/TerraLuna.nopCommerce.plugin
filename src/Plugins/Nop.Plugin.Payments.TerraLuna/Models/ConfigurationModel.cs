using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Payments.TerraLuna.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Nop.Plugin.Payments.TerraLuna.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Plugin.Payments.TerraLuna.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        public int TransactModeId { get; set; }
        [NopResourceDisplayName("Nop.Plugin.Payments.TerraLuna.Fields.TransactMode")]
        public SelectList TransactModeValues { get; set; }
        public bool TransactModeId_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Nop.Plugin.Payments.TerraLuna.Fields.TatumApiKey")]
        public string TatumApiKey { get; set; }
        public bool TatumApiKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Plugin.Payments.TerraLuna.Fields.BitcoinAddress")]
        public string BitcoinAddress { get; set; }
        public bool BitcoinAddress_OverrideForStore { get; set; }
    }
}