using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.TerraLuna.Models;
using Nop.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Payments.TerraLuna.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class TerraLunaController : BasePaymentController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public TerraLunaController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var wMCryptoBitconSettings = await _settingService.LoadSettingAsync<TerraLunaPaymentSettings>(storeScope);

            var model = new ConfigurationModel
            {
                TransactModeId = Convert.ToInt32(wMCryptoBitconSettings.TransactMode),
                AdditionalFee = wMCryptoBitconSettings.AdditionalFee,
                AdditionalFeePercentage = wMCryptoBitconSettings.AdditionalFeePercentage,
                TransactModeValues = await wMCryptoBitconSettings.TransactMode.ToSelectListAsync(),
                TatumApiKey = wMCryptoBitconSettings.TatumApiKey,
                BitcoinAddress = wMCryptoBitconSettings.BitcoinAddress,
                ActiveStoreScopeConfiguration = storeScope
            };
            if (storeScope > 0)
            {
                model.TransactModeId_OverrideForStore = await _settingService.SettingExistsAsync(wMCryptoBitconSettings, x => x.TransactMode, storeScope);
                model.AdditionalFee_OverrideForStore = await _settingService.SettingExistsAsync(wMCryptoBitconSettings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = await _settingService.SettingExistsAsync(wMCryptoBitconSettings, x => x.AdditionalFeePercentage, storeScope);
                model.TatumApiKey_OverrideForStore = await _settingService.SettingExistsAsync(wMCryptoBitconSettings, x => x.TatumApiKey, storeScope);
                model.BitcoinAddress_OverrideForStore = await _settingService.SettingExistsAsync(wMCryptoBitconSettings, x => x.BitcoinAddress, storeScope);
            }

            return View("~/Plugins/Payments.TerraLuna/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var TerraLunaSettings = await _settingService.LoadSettingAsync<TerraLunaPaymentSettings>(storeScope);

            //save settings
            TerraLunaSettings.TransactMode = (TransactMode)model.TransactModeId;
            TerraLunaSettings.AdditionalFee = model.AdditionalFee;
            TerraLunaSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            TerraLunaSettings.TatumApiKey = model.TatumApiKey;
            TerraLunaSettings.BitcoinAddress = model.BitcoinAddress;


            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */

            await _settingService.SaveSettingOverridablePerStoreAsync(TerraLunaSettings, x => x.TransactMode, model.TransactModeId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(TerraLunaSettings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(TerraLunaSettings, x => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(TerraLunaSettings, x => x.TatumApiKey, model.TatumApiKey_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(TerraLunaSettings, x => x.BitcoinAddress, model.BitcoinAddress_OverrideForStore, storeScope, false);



            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        public async Task<IActionResult> Balance()
        {
            var model = new ConfigurationModel
            {
            };
            return View("~/Plugins/Payments.TerraLuna/Views/Balance.cshtml", model);

        }
        #endregion
    }
}