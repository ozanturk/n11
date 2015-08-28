using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Edge;
using NUnit;
using NUnit.Core;
using NUnit.Framework;
using System.Threading;
using Selenium;
using OpenQA.Selenium.Support.UI;

namespace n11
{

    [TestFixture]
    
    public class n11
    {
        string Url = "http://www.n11.com/";
        public static IWebDriver driver = null;

        [SetUp]
        public void start()
        {

            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Url);
            driver.Manage().Window.Maximize();      
        }

        [Test]
        public void login()
        {
            var UserName = "userName";
            var Password = "pass";

            IWebElement Btn_Login = driver.FindElement(By.CssSelector(".btnSignIn"));
            Btn_Login.Click();
            Thread.Sleep(6000);

            IWebElement Input_email = driver.FindElement(By.Id("email"));
            Input_email.SendKeys(UserName);
            Thread.Sleep(1000);

            IWebElement Input_password = driver.FindElement(By.Id("password"));
            Input_password.SendKeys(Password);
            Thread.Sleep(1000);

            //loginButton
            IWebElement Btn_UserLogin = driver.FindElement(By.Id("loginButton"));
            Btn_UserLogin.Click();

            Thread.Sleep(4000);

            IWebElement Kontrol = driver.FindElement(By.XPath(".//*[@id='header']/div/div/div[3]/div[2]/div[1]/div[1]/a[1]"));
            Assert.IsTrue(Kontrol.Text.Equals("Hesabım"), "Kullanıcı girişi başarısız");

            if (Kontrol.Text == "Hesabım")
            {
                Console.WriteLine("Kullanıcı girişi sağlandı");
            }

            Thread.Sleep(3000);
           

        }

        [Test]
        public void SearchSamsung()
        {

            login();

            IWebElement Input_Search = driver.FindElement(By.Id("searchData"));
            Input_Search.SendKeys("samsung");
            Thread.Sleep(4000);

            IWebElement Btn_Search = driver.FindElement(By.ClassName("searchBtn"));
            Btn_Search.Click();
            Thread.Sleep(4000);

            IWebElement Search_Result_Text = driver.FindElement(By.ClassName("resultText"));
            Assert.IsTrue(Search_Result_Text.Text.Contains("Samsung"));
            Thread.Sleep(4000);
            Console.WriteLine("Samsung için arama sonuçları listelendi.");

        }

        [Test]
        public void Paging()
        {
           

            SearchSamsung();
            Thread.Sleep(3000);

            IWebElement Paging_Page2 = driver.FindElement(By.XPath(".//*[@id='contentListing']/div/div/div[2]/div[4]/a[2]"));
            Paging_Page2.Click();
            Thread.Sleep(4000);
            IWebElement currentPage = driver.FindElement(By.ClassName("currentPage"));
            string Sayfa = currentPage.GetAttribute("value").ToString();
            Assert.True(Sayfa.Equals("2"),"2. Sayfaya ulaşılamadı");
            Thread.Sleep(4000);
            Console.WriteLine("Paging kontrolü kullanıldı ve 2. sayfaya ulaşıldı.");

        }

        [Test]
        public void SepeteAt()
        {
            Thread.Sleep(3000);

            Paging();
            Thread.Sleep(3000);

            IWebElement List_3rdItem = driver.FindElement(By.XPath(".//*[@id='view']/ul/li[3]"));
            IWebElement ClickItem = List_3rdItem.FindElement(By.ClassName("productName"));
            var sepete_atilan_item = ClickItem.Text;
            ClickItem.Click();
            Thread.Sleep(4000);
            Console.WriteLine("Ürün seçildi ve detay ekranına ulaşıldı...");

            IWebElement SelectBox = driver.FindElement(By.XPath(".//*[@id='skuArea']/fieldset"));
            IWebElement SelectColorCombo = SelectBox.FindElement(By.TagName("select"));

            SelectElement selectColor = new SelectElement(SelectColorCombo);
            selectColor.SelectByIndex(1);
            Console.WriteLine("Ürün rengi seçildi...");

            IWebElement Btn_Basket = driver.FindElement(By.CssSelector(".btn.btnGrey.btnAddBasket"));
            Btn_Basket.Click();
            Thread.Sleep(3000);
            Console.WriteLine("Ürün speete atıldı...");
            IWebElement Sepet = driver.FindElement(By.CssSelector(".myBasket"));
            Sepet.Click();
            Thread.Sleep(8000);
            Console.WriteLine("Sepet içerisine girildi...");
            IWebElement productDetail = driver.FindElement(By.ClassName("productDetail"));
            var title = productDetail.FindElement(By.TagName("h4"));

            Assert.That(title.Text, Is.EqualTo(sepete_atilan_item));
            Console.WriteLine("Sepetteki ürün ile seçilen ürünün aynı ürün olduğu doğrulandı...");
            IWebElement Btn_Sil = driver.FindElement(By.ClassName("remove"));
            Btn_Sil.Click();
            Thread.Sleep(4000);
            Console.WriteLine("Ürün silme işlemi gerçekleştirildi...");

            IWebElement SilinmeOnayi = driver.FindElement(By.XPath(".//*[@id='content']/div/div[1]/div[1]/span[1]"));
            Assert.True(SilinmeOnayi.Text.Contains("Sepetiniz Boş"));
            Console.WriteLine("Ürün silinme onayı alındı...");
            Thread.Sleep(3000);
            

        }


        [TearDown]
        public void finish()
        {

            driver.Close();
            driver.Quit();
        }
        
    }
}
