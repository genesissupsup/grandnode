﻿using Grand.Plugin.Payments.PayInStore.Models;
using Grand.Services.Configuration;
using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Grand.Services.Localization;

namespace Grand.Plugin.Payments.PayInStore.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    public class PaymentPayInStoreController : BasePaymentController
    {
        private readonly ISettingService _settingService;
        private readonly PayInStorePaymentSettings _payInStorePaymentSettings;
        private readonly ILocalizationService _localizationService;

        public PaymentPayInStoreController(ISettingService settingService, PayInStorePaymentSettings payInStorePaymentSettings, ILocalizationService localizationService)
        {
            this._settingService = settingService;
            this._payInStorePaymentSettings = payInStorePaymentSettings;
            this._localizationService = localizationService;
        }

        [AuthorizeAdmin]
        public ActionResult Configure()
        {
            var model = new ConfigurationModel();
            model.DescriptionText = _payInStorePaymentSettings.DescriptionText;
            model.AdditionalFee = _payInStorePaymentSettings.AdditionalFee;
            model.AdditionalFeePercentage = _payInStorePaymentSettings.AdditionalFeePercentage;

            return View("~/Plugins/Payments.PayInStore/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _payInStorePaymentSettings.DescriptionText = model.DescriptionText;
            _payInStorePaymentSettings.AdditionalFee = model.AdditionalFee;
            _payInStorePaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            _settingService.SaveSetting(_payInStorePaymentSettings);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }
    }
}