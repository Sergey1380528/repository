
                //https://github.com/jamietre/CsQuery/
                
                List<IG_DocumentItem> cDocuments = new List<IG_DocumentItem>();

                //todo убрать костыль, решить проблему с кодировкой m_strHtml
                string strHtml = System.IO.File.ReadAllText(m_strUrl, Encoding.GetEncoding(1251));

                var cPage = CQ.Create(strHtml); //FromFile(m_strUrl);
                var cListTd = cPage.Select("td.documentsOrder.documentsInfo");
                foreach (IDomObject cTd in cListTd)
                {
                    CQ cItem = CQ.Create(cTd).Parent();

                    //Заголовок извещения
                    string strTitle = cItem.Select("td.documentsOrder.documentsInfo").Text();
                    if (!string.IsNullOrEmpty(strTitle))
                    {
                        strTitle = strTitle.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                    }

                    //Действующая редакция
                    bool bIsUsed = !cItem.Select("td.nonperformingedition").Any();

                    //Дата публикации
                    string strDate1 = cItem.Select(". documentsInfoDate").Html();
                    string strDate = cItem.Select(".documentsInfoDate").Text();
                    if (!string.IsNullOrEmpty(strDate))
                    {
                        strDate = strDate.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                    }

                    //Дата публикации
                    string strPrintForm = cItem.Select(".printForm").Html();//Attr("href");
                    


                    cDocuments.Add(new IG_DocumentItem
                    {
                        Title = strTitle,
                        IsUsed = bIsUsed,
                        Date = strDate,
                        PrintForm = strPrintForm

                    });
                }
