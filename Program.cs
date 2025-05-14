using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

class Program
{
    private static int count = 1;
    static void Main()
    {
        // Настройки ChromeOptions
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--disable-blink-features=AutomationControlled"); // Скрываем автоматизацию
        chromeOptions.AddArgument("--disable-infobars"); // Убираем сообщение "Chrome is being controlled..."
        chromeOptions.AddArgument("--start-maximized"); // Открываем браузер на весь экран

        // Инициализируем драйвер
        using (var driver = new ChromeDriver(chromeOptions))
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            for (int i = 0; i < count; i++)
            {
                // Открываем Яндекс Форму
                driver.Navigate().GoToUrl("https://forms.yandex.ru/u/{form_id}/");

                try
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[type=\"submit\"]")));
                }
                catch
                {
                    Console.WriteLine("Страница не загружена? Мб CAPTCHA");
                }

                // 1. Если появилась CAPTCHA "Я не робот" → кликаем вручную или ждём 10 сек
                try
                {
                    var captchaButton = driver.FindElement(By.CssSelector(".CheckboxCaptcha-Button"));
                    captchaButton.Click();
                    Console.WriteLine("CAPTCHA была нажата!");
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[type=\"submit\"]"))); // Ждём, пока пройдёт проверка
                }
                catch { }

                // 2. Заполняем форму (пример для вашей структуры)
                try
                {
                    //id ответа

                    var inputField = driver.FindElement(By.CssSelector("input[value=\"{id}\"]"));
                    inputField.Click();
                }
                catch
                {
                    Console.WriteLine("Поле не найдено!");
                }

                // 3. Отправляем форму
                try
                {
                    var submitButton = driver.FindElement(By.CssSelector("button[type=\"submit\"]"));
                    submitButton.Click();
                    //для дебага или просто чтобы замедлить выполнение, должно работать и без
                    //wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div[class=\"SuccessMessage-Buttons\"]")));
                    Console.WriteLine($"{i+1}/{count} Форма отправлена!");
                }
                catch
                {
                    Console.WriteLine($"{i+1}/{count} чета не так!");
                }
            }
            Console.WriteLine("Готово!"); Console.Read();
        }
    }
}