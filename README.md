

            try
            {

                string strResult = string.Empty;

                //https://github.com/jamietre/CsQuery/

　
                //http://zakupki.gov.ru/epz/order/notice/ea44/view/documents.html?regNumber=0320100020717000132

                //todo убрать костыль, решить проблему с кодировкой m_strHtml
                string strHtml = File.ReadAllText(@"D:\zakupki.gov.ru\zakupki.gov.ru.txt ", Encoding.GetEncoding(1251));

                var cPage = CQ.Create(strHtml);
                var cPrintFormList = cPage.Select("a.printForm");

                //Отладка
                //System.IO.File.WriteAllText(@"D:\zakupki.gov.ru\cItem.txt", cListTd.Html(), Encoding.GetEncoding(1251));

                foreach (IDomObject cPrintFormItem in cPrintFormList)
                {
                    //получение родительского элемента
                    //string strTdParentHtml = cTd.ParentNode.OuterHTML;
                    string strTr = cPrintFormItem.ParentNode.ParentNode.OuterHTML;
                    CQ cTr = CQ.Create(strTr);

　
                    //Действующая редакция
                    bool bIsUsed = !cTr.Select("td.nonperformingedition").Any();

                    //Пропуск не действующие редакции
                    if (!bIsUsed)
                    {
                        continue;
                    }

　
　
                    //Заголовок извещения расположен в td и сдедует вторым т.у. за печатной формой
                    string strTitle = string.Empty;
                    if (cTr.Children("td").Count() > 1)
                    {
                        string strTdHtml = cTr.Children("td")[1].OuterHTML;
                        CQ cTd = CQ.Create(strTdHtml);
                        strTitle= cTd.Text().Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                    }

                    /*
                    //Заголовок извещения
                    string strTitle = cTr.Select("td.documentsOrder.documentsInfo").Text();
                    if (!string.IsNullOrEmpty(strTitle))
                    {
                        strTitle = strTitle.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                    }
                    */

　
                    //Дата публикации
                    string strDate = cTr.Select(".documentsInfoDate").Text();
                    strDate = strDate.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                    /*
                    if (!string.IsNullOrEmpty(strDate))
                    {
                        strDate = strDate.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                    }
                    */

　
                    //Ссылка на печатную форму
                    string strPrintForm = string.Empty;
                    var cPrintForm = cTr.Select("a.printForm");
                    if (cPrintForm.HasAttr("href"))
                    {
                        strPrintForm = cPrintForm.Attr("href");
                    }

　
                    //Вложения
                    string strLinks = string.Empty;
                    /*
                    var cTdAttachments = cItem.Select("td.crossBreakWord");
                    foreach (IDomObject cTdAttachment in cTdAttachments)
                    {
                        string strAttachmentName = string.Empty;
                        string strAttachmentHref = String.Empty;

                        //получение элемента
                        CQ cAttachment = CQ.Create(cTdAttachment.OuterHTML);

                        var cLinks = cAttachment.Select("a");

                        //Наименование
                        strAttachmentName = cLinks.Text().Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                        if (cLinks.HasAttr("title"))
                        {
                            strAttachmentName = $"{strAttachmentName} ({cLinks.Attr("title")})";
                        }

                        //Ссылка
                        if (cLinks.HasAttr("href"))
                        {
                            strAttachmentHref = cLinks.Attr("href");
                        }

                        strLinks = $"{strLinks}\n\t{strAttachmentName}->{strAttachmentHref}";
                    }
                    */

                    //Еще один способ поиска вложений
                    var cAAttachments = cTr.Select("a");
                    foreach (IDomObject cAAttachment in cAAttachments)
                    {
                        string strAttachmentName = string.Empty;
                        string strAttachmentHref = String.Empty;

                        //получение элемента
                        //CQ cAttachment = CQ.Create(cAAttachment.OuterHTML);
                        //var cLinks = cAttachment.Select("a");
                        CQ cLinks = CQ.Create(cAAttachment.OuterHTML);

                        //Наименование
                        strAttachmentName = cLinks.Text().Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
                        if (cLinks.HasAttr("title"))
                        {
                            strAttachmentName = $"{strAttachmentName} ({cLinks.Attr("title")})";
                        }

                        //Ссылка
                        if (cLinks.HasAttr("href"))
                        {
                            strAttachmentHref = cLinks.Attr("href");
                        }

                        if (!strAttachmentHref.Contains("download/priz/file.html?"))
                        {
                            continue;
                        }

                        strLinks = $"{strLinks}\n\t{strAttachmentName}->{strAttachmentHref}";
                    }

　
　
                    Console.WriteLine(
$@"strTitle-{strTitle} 
bIsUsed-{bIsUsed}
strDate-{strDate}
strPrintForm-{strPrintForm}
strLinks:{strLinks}
");

                    strResult =
$@"{strResult}

strTitle-{strTitle} 
bIsUsed-{bIsUsed}
strDate-{strDate}
strPrintForm-{strPrintForm}
strLinks:{strLinks}
";

　
                }

                

                System.IO.File.WriteAllText(@"D:\zakupki.gov.ru\cResult.txt", strResult, Encoding.GetEncoding(1251));

　
　
　
　
                Console.ReadKey();
            }
            catch (Exception cException)
            {
                throw new Exception("Не удалось получить изменения", cException);
            }
        
