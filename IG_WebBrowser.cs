
namespace GetWebPageChanges.Logic
{
    /// <summary>
    /// Âñïîìîãàòåëüíûé êëàññ äëÿ ðàáîòû ñ Web áðàóçåðîì
    /// Example: IG_WebBrowser cWebBrowser = new IG_WebBrowser(strUrl)
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
        /// Êîíñòðóèðîâàíèå îáúåêòà
        /// </summary>
        /// <param name="strUrl">Url àäðåñ</param>
        /// <param name="nTimeOut">Âðåìÿ îæèäàíèÿ îòâåòà (â ñåêóíäàõ)</param>
        public IG_WebBrowser(string strUrl, int nTimeOut = 60)
        {
            try
            {
                m_strUrl = strUrl;

                m_cWebPage = new WebBrowser();
                //áëîêèðóåì âñïëûâàþùèå îêíà êîòîðûå ìîãóò îæèäàòü äåéñòâèé ïîëüçîâàòåëÿ
                m_cWebPage.ScriptErrorsSuppressed = false;

                m_cWebPage.DocumentCompleted += WebPageDocumentCompleted;

                m_cWebPage.Navigate(m_strUrl);

                DateTime cDateStart = DateTime.Now;
                while (!m_bIsDocumentCompleted)
                {
                    //Îáðàáîòêà âñåõ ñîîáùåíèé íàõîäÿùèõñÿ â î÷åðåäè
                    Application.DoEvents();

                    TimeSpan cTimeSpan = DateTime.Now - cDateStart;

                    if (cTimeSpan.TotalSeconds > nTimeOut)
                    {
                        throw new Exception($"Ïðåâûøåíî âðåìÿ îæèäàíèÿ ñåðâåðà ({m_nTimeOut} ñåê.)");
                    }
                }
            }
            catch (Exception cException)
            {
                throw new Exception($"Íå óäàëîñü ñêîíñòðóèðîâàòü îáúåêò WebPage ïî ññûëêå: \"{m_strUrl}\"", cException);
            }
        }



        /// <summary>
        /// Ñîáûòèå ïîëíîé çàãðóçêè ñòðàíèöû
        /// </summary>
        private void WebPageDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            m_bIsDocumentCompleted = true;
        }



        public void Dispose()
        {
            m_cWebPage.DocumentCompleted -= WebPageDocumentCompleted;
            m_cWebPage.Dispose();
        }
    }
}
