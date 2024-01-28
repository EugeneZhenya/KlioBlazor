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
                    errorDescription = "������ �� ��� ��������� �����.";
                    break;
                case 401:
                    errorDescription = "����� �� ��� ����������, ������� ���� �� ������� ������ �������� �����.";
                    break;
                case 402:
                    errorDescription = "����� �� ���� ���� ��������� ����, ���� �� �� �������� ������.";
                    break;
                case 403:
                    errorDescription = "��� ��������� � �����������.";
                    break;
                case 404:
                    errorDescription = "������ �� ���� ������ �������� ������.";
                    break;
                case 405:
                    errorDescription = "����� ������ ��� ���������� � �� ���� ���� ������������.";
                    break;
                case 406:
                    errorDescription = "������ �� � ���� ��������.";
                    break;
                case 407:
                    errorDescription = "����� �� �� ���������� �������� ����� ��� �����-�������.";
                    break;
                case 408:
                    errorDescription = "��� ���������� ������ ���������.";
                    break;
                case 409:
                    errorDescription = "�������� ������� ������ �� �������� ������ �������.";
                    break;
                case 410:
                    errorDescription = "������ ����� ����������� �� ������.";
                    break;
                case 411:
                    errorDescription = "C����� ������������ �������� ����� ��� ��������� ������� ��������.";
                    break;
                case 412:
                    errorDescription = "������ �� ������� ���� ��������.";
                    break;
                case 413:
                    errorDescription = "����� �������� ��������� �������.";
                    break;
                case 414:
                    errorDescription = "����� �� ������� ������ �������� �������.";
                    break;
                case 415:
                    errorDescription = "������ ����� �� ����������� ��������.";
                    break;
                case 416:
                    errorDescription = "������ �� � ���� ��������� ������� ��������.";
                    break;
                case 417:
                    errorDescription = "���������� �� ���� ���� ��������.";
                    break;
                case 418:
                    errorDescription = "������ �� ���� ����������� ����, ���� �� �� ������.";
                    break;
                case 422:
                    errorDescription = "������� �� ������� �������� ���������� �����.";
                    break;
                case 429:
                    errorDescription = "�� �������� �������� ������ �������� ���������� ����.";
                    break;
                case 431:
                    errorDescription = "���� ��������� �������.";
                    break;
                case 451:
                    errorDescription = "������ ����������� � ��������� ������.";
                    break;
                case 500:
                    errorDescription = "�������� ������� �������.";
                    break;
                case 501:
                    errorDescription = "����� ������ �� ����������� ��������.";
                    break;
                case 502:
                    errorDescription = "���� ��� ����� ������� ����� ������� �� ���������� �������.";
                    break;
                case 503:
                    errorDescription = "������ �� ������� �������� �����.";
                    break;
                case 504:
                    errorDescription = "���� ��� ����� �� ���� ������ �������� ������.";
                    break;
                case 505:
                    errorDescription = "����� ������ �� ����������� ��������.";
                    break;
                case 511:
                    errorDescription = "�� ���� ������ ��������������, ��� �������� ������ �� �����.";
                    break;
            }
        }
    }
}
