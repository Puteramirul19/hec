using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hec.Entities;
using Hec.Integrations;

namespace Hec.Tests
{
    [TestClass]
    public class SspTests
    {
        [TestMethod]
        public void TestUserTokenEncryption()
        {
            var svc = new SspService(null, null, null, "TNB_SSP_SSO_HEC");

            Assert.AreEqual("Ik1BSJ29mm8Pt2yRFOkBcrQNvK7PVq/0baFxxRk8DEc642E6BwH49pKTtmljLYqJcbeMoXR9DwYgw7cPClUkAw==",
                svc.GenerateTokenSignature("TESTER@TNB.COM.MY", "4AB9708304E3406DAC6C165D0D5B403C"));

            Assert.AreEqual("Ik1BSJ29mm8Pt2yRFOkBcrQNvK7PVq/0baFxxRk8DEc642E6BwH49pKTtmljLYqJcbeMoXR9DwYgw7cPClUkAw==",
                svc.GenerateTokenSignature("tester@tnb.com.my", "4ab9708304E3406DAC6C165D0D5B403C"));
        }


        [TestMethod]
        public void TestBuildTokenVerificationUrl()
        {
            var svc = new SspService("http://test.mytnb.com.my:8011/SSO/WebApi/TokenVerification", "SSP_SSO_HEC", "T_VERIFY", "TNB_SSP_SSO_HEC");

            Assert.AreEqual(
                "http://test.mytnb.com.my:8011/SSO/WebApi/TokenVerification?CHANNEL_KEY=SSP_SSO_HEC&ACTION_KEY=T_VERIFY&USERTOKEN=4AB9708304E3406DAC6C165D0D5B403C&SIGNATURE=Ik1BSJ29mm8Pt2yRFOkBcrQNvK7PVq/0baFxxRk8DEc642E6BwH49pKTtmljLYqJcbeMoXR9DwYgw7cPClUkAw==",
                svc.BuildTokenVerificationUrl("TESTER@TNB.COM.MY", "4AB9708304E3406DAC6C165D0D5B403C"));

        }
    }
}
