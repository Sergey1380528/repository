using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace GetWebPageChanges.Logic
{
    /// <summary>
    /// ��������������� ����� ��� ������ � Web ���������
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
        /// ��������������� �������
        /// </summary>
        /// <param name="strUrl">Url �����</param>
        /// <param name="nTimeOut">����� �������� ������ (� ��������)</param>
        public IG_WebBrowser(string strUrl, int nTimeOut = 60)
        {
            try
            {
                m_strUrl = strUrl;

                m_cWebPage = new WebBrowser();
                //��������� ����������� ���� ������� ����� ������� �������� ������������
                m_cWebPage.ScriptErrorsSuppressed = false;

                m_cWebPage.DocumentCompleted += WebPageDocumentCompleted;

                m_cWebPage.Navigate(m_strUrl);

                DateTime cDateStart = DateTime.Now;
                while (!m_bIsDocumentCompleted)
                {
                    //��������� ���� ��������� ����������� � �������
                    Application.DoEvents();

                    TimeSpan cTimeSpan = DateTime.Now - cDateStart;

                    if (cTimeSpan.TotalSeconds > nTimeOut)
                    {
                        throw new Exception($"��������� ����� �������� ������� ({m_nTimeOut} ���.)");
                    }
                }
            }
            catch (Exception cException)
            {
                throw new Exception($"�� ������� ��������������� ������ WebPage �� ������: \"{m_strUrl}\"", cException);
            }
        }



        /// <summary>
        /// ������� ������ �������� ��������
        /// </summary>
        private void WebPageDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            m_bIsDocumentCompleted = true;
        }



        /// <summary>
        /// �������� ������� � �������� �����
        /// </summary>
        /// <returns>���������� ����� ������� ������������� � ��������</returns>
        public string GetText()
        {
            try
            {
                //���������� ����� � ������ ������
                string strOldClipboardText = Clipboard.GetText();

                //��������� ����� ��� ���������
                m_cWebPage.Document.ExecCommand("SelectAll", false, null);
                m_cWebPage.Document.ExecCommand("Copy", false, null);
                string strResult = Clipboard.GetText();


                //���� ����� ������������� � �������� ��������� � ������ ������� � ������, �� ��� �������� �����
                if (strResult == strOldClipboardText)
                {
                    strResult = string.Empty;
                }


                //�������������� ������� ����� � ������ ������
                if (string.IsNullOrEmpty(strOldClipboardText))
                {
                    Clipboard.Clear();
                }
                else
                {
                    Clipboard.SetText(strOldClipboardText);
                }

                //�������� ���������� ���������
                for (int i = 0; i < 100; i++)
                {
                    strResult = strResult.Replace("\r", "\n");
                    strResult = strResult.Replace("\n ", "\n");
                    strResult = strResult.Replace(" \n", "\n");
                    strResult = strResult.Replace("\n\n", "\n");
                }
                //�������� �������� � ������ � ����� ������
                strResult = strResult.Trim('\n');

                return strResult;
            }
            catch (Exception cException)
            {
                throw new Exception($"�� ������� �������� ����� �� ��������.", cException);
            }
        }



        /// <summary>
        /// �������� html �������� ��������
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
                throw new Exception($"�� ������� �������� html �������� ��������.", cException);
            }
        }



        /// <summary>
        /// �������������� �������� (������ ������� � ����� ������)
        /// </summary>
        /// <param name="strWebBrowserDocumentText"></param>
        /// <param name="bNeedRemoveHead">������� ���������</param>
        /// <param name="bNeedRemoveScript">������� �������</param>
        /// <param name="bNeedRemoveStyle">������� �����</param>
        /// <returns>����������������� html �������� (������ ��� � ����� ������)</returns>
        public string HtmlFormat(string strWebBrowserDocumentText, bool bNeedRemoveHead = true, bool bNeedRemoveScript = true, bool bNeedRemoveStyle = true)
        {
            try
            {
                string strHtml = strWebBrowserDocumentText;

                //�������� ���������
                if (bNeedRemoveHead)
                {
                    Regex cRegexRemoveHead = new Regex(@"<head[^>]*>[\s\S]*?</head>");
                    strHtml = cRegexRemoveHead.Replace(strHtml, "");
                }

                //�������� ��������
                if (bNeedRemoveScript)
                {
                    Regex cRegexRemoveScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
                    //Regex rRemScript = new Regex("<script.*?</script>");
                    strHtml = cRegexRemoveScript.Replace(strHtml, "");
                }

                //�������� ������
                if (bNeedRemoveStyle)
                {
                    Regex cRegexRemoveStyle = new Regex(@"<style[^>]*>[\s\S]*?</style>");
                    strHtml = cRegexRemoveStyle.Replace(strHtml, "");
                }

                //�������� ���������
                strHtml = strHtml.Replace("\r", "");
                strHtml = strHtml.Replace("\n", "");

                //�������� ���������� ��������
                for (int i = 0; i < 100; i++)
                {
                    strHtml = strHtml.Replace("  ", " ");
                }

                //����������� ���� �� ����� ������
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

                //�������� ��������� � ������ � �����
                strHtml = strHtml.Trim('\n');
                strHtml = strHtml.Trim('\r');

                return strHtml;
            }
            catch (Exception cException)
            {
                throw new Exception("�� ������� ��������������� html �������� ��������", cException);
            }
        }



        public void Dispose()
        {
            m_cWebPage.DocumentCompleted -= WebPageDocumentCompleted;
            m_cWebPage.Dispose();
        }
    }
}