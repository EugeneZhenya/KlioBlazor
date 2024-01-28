using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages
{
    public class StatusCodeModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int errorId { get; set; }
        public string errorDescription { get; set; }

        public void OnGet()
        {
            switch (errorId)
            {
                case 400:
                    errorDescription = "Сервер не зміг розпізнати запит.";
                    break;
                case 401:
                    errorDescription = "Запит не був оброблений, оскільки йому не вистачає дійсних облікових даних.";
                    break;
                case 402:
                    errorDescription = "Запит не може бути виконаний доти, доки Ви не здійсните оплату.";
                    break;
                case 403:
                    errorDescription = "Вам відмовлено в авторизації.";
                    break;
                case 404:
                    errorDescription = "Сервер не може знайти потрібний ресурс.";
                    break;
                case 405:
                    errorDescription = "Метод запиту був відключений і не може бути використаний.";
                    break;
                case 406:
                    errorDescription = "Сервер не в змозі відповісти.";
                    break;
                case 407:
                    errorDescription = "Запит не має достовірних облікових даних для проксі-сервера.";
                    break;
                case 408:
                    errorDescription = "Час очікування запиту вичерпано.";
                    break;
                case 409:
                    errorDescription = "Виявлено конфлікт запиту із поточним станом сервера.";
                    break;
                case 410:
                    errorDescription = "Ресурс більше недоступний на сервері.";
                    break;
                case 411:
                    errorDescription = "Cервер відмовляється прийняти запит без вихначеної довжини контенту.";
                    break;
                case 412:
                    errorDescription = "Доступ до ресурсу було відхилено.";
                    break;
                case 413:
                    errorDescription = "Запит перевищує обмеження сервера.";
                    break;
                case 414:
                    errorDescription = "Запит має занадто довгий вказівник ресурсу.";
                    break;
                case 415:
                    errorDescription = "Формат вмісту не підтримується сервером.";
                    break;
                case 416:
                    errorDescription = "Сервер не в змозі обслужити запитані діапазони.";
                    break;
                case 417:
                    errorDescription = "Очікування не може бути виконано.";
                    break;
                case 418:
                    errorDescription = "Сервер не може приготувати каву, тому що він чайник.";
                    break;
                case 422:
                    errorDescription = "Серверу не вдалося обробити інструкції вмісту.";
                    break;
                case 429:
                    errorDescription = "Ви надіслалм забагато запитів впродовж останнього часу.";
                    break;
                case 431:
                    errorDescription = "Поля заголовку завеликі.";
                    break;
                case 451:
                    errorDescription = "Ресурс недоступний з юридичних причин.";
                    break;
                case 500:
                    errorDescription = "Внутрішня помилка сервера.";
                    break;
                case 501:
                    errorDescription = "Метод запиту не підтримується сервером.";
                    break;
                case 502:
                    errorDescription = "Шлюз або проксі отримав хибну відповідь від висхідного сервера.";
                    break;
                case 503:
                    errorDescription = "Сервер не готовий обробити запит.";
                    break;
                case 504:
                    errorDescription = "Шлюз або проксі не може вчасно отримати відповіді.";
                    break;
                case 505:
                    errorDescription = "Версія запиту не підтримується сервером.";
                    break;
                case 511:
                    errorDescription = "Ви маєте пройти аутентифікацію, щоб отримати доступ до мережі.";
                    break;
            }
        }
    }
}
