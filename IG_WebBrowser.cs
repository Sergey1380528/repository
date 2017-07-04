using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace GetWebPageChanges.Logic
{
    /// <summary>
    /// Вспомогательный класс для работы с Web браузером
    /// </summary>
    public class IG_WebBrowser : IDisposable
    {
        private int m_nTimeOut;
        private WebBrowser m_cWebPage;
        private bool m_bIsDocumentCompleted = false;
        private string m_strUrl;

        public WebBrowser WebPage
        {
            get { return m_cWebPage; }
        }

        /// <summary>
        /// Конструирование объекта
        /// </summary>
        /// <param name="strUrl">Url адрес</param>
        /// <param name="nTimeOut">Время ожидания ответа (в секундах)</param>
        public IG_WebBrowser(string strUrl, int nTimeOut = 60)
        {
            try
            {
                m_strUrl = strUrl;

                m_cWebPage = new WebBrowser();
                //блокируем всплывающие окна которые могут ожидать действий пользователя
                m_cWebPage.ScriptErrorsSuppressed = false;

                m_cWebPage.DocumentCompleted += WebPageDocumentCompleted;

                m_cWebPage.Navigate(m_strUrl);

                DateTime cDateStart = DateTime.Now;
                while (!m_bIsDocumentCompleted)
                {
                    //Обработка всех сообщений находящихся в очереди
                    Application.DoEvents();

                    TimeSpan cTimeSpan = DateTime.Now - cDateStart;

                    if (cTimeSpan.TotalSeconds > nTimeOut)
                    {
                        throw new Exception($"Превышено время ожидания сервера ({m_nTimeOut} сек.)");
                    }
                }
            }
            catch (Exception cException)
            {
                throw new Exception($"Не удалось сконструировать объект WebPage по ссылке: \"{m_strUrl}\"", cException);
            }
        }



        /// <summary>
        /// Событие полной загрузки страницы
        /// </summary>
        private void WebPageDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            m_bIsDocumentCompleted = true;
        }



        /// <summary>
        /// Получить видимый в браузере текст
        /// </summary>
        /// <returns>Возвращает текст видимый пользователем в браузере</returns>
        public string GetText()
        {
            try
            {
                //Запоминаем текст в буфере обмена
                string strOldClipboardText = Clipboard.GetText();

                //Сохраняем текст для сравнения
                m_cWebPage.Document.ExecCommand("SelectAll", false, null);
                m_cWebPage.Document.ExecCommand("Copy", false, null);
                string strResult = Clipboard.GetText();


                //Если текст скопированный с страницы совпадает с старым текстом в буфере, то Веб страница пуста
                if (strResult == strOldClipboardText)
                {
                    strResult = string.Empty;
                }


                //Востанавливаем прежний текст в буфере обмена
                if (string.IsNullOrEmpty(strOldClipboardText))
                {
                    Clipboard.Clear();
                }
                else
                {
                    Clipboard.SetText(strOldClipboardText);
                }

                //Удаление избыточных переносов
                for (int i = 0; i < 100; i++)
                {
                    strResult = strResult.Replace("\r", "\n");
                    strResult = strResult.Replace("\n ", "\n");
                    strResult = strResult.Replace(" \n", "\n");
                    strResult = strResult.Replace("\n\n", "\n");
                }
                //Удаление отступов в начале и конце строки
                strResult = strResult.Trim('\n');

                return strResult;
            }
            catch (Exception cException)
            {
                throw new Exception($"Не удалось получить текст со страницы.", cException);
            }
        }



        /// <summary>
        /// Получить html разметку страницы
        /// </summary>
        /// <returns></returns>
        public string GetHtml()
        {
            try
            {
                /*
                string strHtmlEncod;
                using (StreamReader sr = new StreamReader(WebPage.DocumentStream, Encoding.GetEncoding("windows-1251")))
                {
                    strHtmlEncod = sr.ReadToEnd();
                }
                */


                string strResult = HtmlFormat(WebPage.DocumentText);

                return strResult;
            }
            catch (Exception cException)
            {
                throw new Exception($"Не удалось получить html разметку страницы.", cException);
            }
        }



        /// <summary>
        /// Форматирование разметки (каждый элемент с новой строки)
        /// </summary>
        /// <param name="strWebBrowserDocumentText"></param>
        /// <param name="bNeedRemoveHead">Удалить заголовок</param>
        /// <param name="bNeedRemoveScript">Удалить скрипты</param>
        /// <param name="bNeedRemoveStyle">Удалить стили</param>
        /// <returns>Отформатированную html разметку (каждый тег с новой строки)</returns>
        public string HtmlFormat(string strWebBrowserDocumentText, bool bNeedRemoveHead = true, bool bNeedRemoveScript = true, bool bNeedRemoveStyle = true)
        {
            try
            {
                string strHtml = strWebBrowserDocumentText;

                //Удаление заголовка
                if (bNeedRemoveHead)
                {
                    Regex cRegexRemoveHead = new Regex(@"<head[^>]*>[\s\S]*?</head>");
                    strHtml = cRegexRemoveHead.Replace(strHtml, "");
                }

                //Удаление скриптов
                if (bNeedRemoveScript)
                {
                    Regex cRegexRemoveScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
                    //Regex rRemScript = new Regex("<script.*?</script>");
                    strHtml = cRegexRemoveScript.Replace(strHtml, "");
                }

                //Удаление стилей
                if (bNeedRemoveStyle)
                {
                    Regex cRegexRemoveStyle = new Regex(@"<style[^>]*>[\s\S]*?</style>");
                    strHtml = cRegexRemoveStyle.Replace(strHtml, "");
                }

                //Удаление переносов
                strHtml = strHtml.Replace("\r", "");
                strHtml = strHtml.Replace("\n", "");

                //Удаление избыточных пробелов
                for (int i = 0; i < 100; i++)
                {
                    strHtml = strHtml.Replace("  ", " ");
                }

                //Автоперенос тега на новую строку
                for (int i = 0; i < 100; i++)
                {
                    strHtml = strHtml.Replace(" >", ">");
                    strHtml = strHtml.Replace("> ", ">");
                    strHtml = strHtml.Replace(" <", "<");
                    strHtml = strHtml.Replace("< ", "<");
                }
                strHtml = strHtml.Replace("<", "\n<");
                strHtml = strHtml.Replace(">", ">\n");
                strHtml = strHtml.Replace("\n\n", "\n");

                //удаление переносов в начале и конце
                strHtml = strHtml.Trim('\n');
                strHtml = strHtml.Trim('\r');

                return strHtml;
            }
            catch (Exception cException)
            {
                throw new Exception("Не удалось отформатировать html разметку страницы", cException);
            }
        }



        public void Dispose()
        {
            m_cWebPage.DocumentCompleted -= WebPageDocumentCompleted;
            m_cWebPage.Dispose();
        }
    }
}