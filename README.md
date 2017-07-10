<!--
http://zakupki.gov.ru/epz/order/notice/ea44/view/documents.html?regNumber=0534600011617000001
Основной контейнер
<div class="contentTabBoxBlock">
заголовок раздела
 <h2 class="noticeBoxH2">Извещение, изменения извещения о проведении электронного аукциона, документация об электронном аукционе</h2>
содержимое раздела
 <div class="noticeTabBoxWrapper">
  <table id="notice-documents">
Печатная форма   недействующие редакции
   <tr class="inactive-order-info" style="display: table-row;">
    <a title="Печатная форма" class="printForm" href="/epz/order/notice/printForm/view.html?printFormId=44133433" target="_blank"></a>
    <a class="nonperformingedition" href="http://zakupki.gov.ru/44fz/filestore/public/1.0/download/priz/file.html?uid=53449EB361A5010EE0530A86121FE7DA" title="АД Тангуй уголь.docx (171.23 Кб)">АД Тангуй уголь</a>
   действующие редакции
   <tr>    <td style="white-space: nowrap; vertical-align: top; padding-right: 0">
     <a title="Печатная форма" class="printForm" href="/epz/order/notice/printForm/view.html?printFormId=44636129" target="_blank"></a>
     <a href="http://zakupki.gov.ru/44fz/filestore/public/1.0/download/priz/file.html?uid=53F155F6C79C0172E0530A86121FEF98" title="АД Тангуй уголь2.docx (171.73 Кб)">АД Тангуй уголь2</a>
     <a href="http://zakupki.gov.ru/44fz/filestore/public/1.0/download/priz/file.html?uid=53ED2FF386B700EAE0530A86121F37B8" title="Внесенные изменения в документацию аукциона ТКС.docx (13.06 Кб)">Внесенные изменения в документацию аукциона ТКС</a>
   ДРУГОЙ РАЗДЕЛ 
   действующие редакции
   <tr class="">
    <td style="white-space: nowrap; vertical-align: top; padding-right: 0">
     <a title="Печатная форма" class="printForm" href="/epz/order/notice/printForm/view.html?printFormId=44464291" target="_blank"></a>              <a href="http://zakupki.gov.ru/44fz/filestore/public/1.0/download/priz/file.html?uid=53A1BA29634A00BAE0530A86121F1169" title="Ответ на запрос разъяснений Тангуй.docx (14.98 Кб)">Ответ на запрос разъяснений Тангуй</a>
-->



            //изменения в ЖУРНАЛЕ СОБЫТИЙ
            try
            {
                Dictionary<string, string> cDictLast = new Dictionary<string, string>();
                cDictLast.Add("2017-07-09", "событие 1");
                cDictLast.Add("2017-07-10", "событие 2");
                cDictLast.Add("2017-07-10_", "событие 3");

                Dictionary<string, string> cDictNew = new Dictionary<string, string>();
                cDictNew.Add("2017-07-09", "событие 1");
                cDictNew.Add("2017-07-10", "событие 2");
                cDictNew.Add("2017-07-11", "событие 3");
                //cDictNew.Add("2017-07-12", "событие 3");

                List<string> cLast = new List<string>();
                foreach (KeyValuePair<string, string> cItem in cDictLast)
                {
                    cLast.Add($"{cItem.Key.Replace("_", "")} {cItem.Value}");
                }

                List<string> cNew = new List<string>();
                foreach (KeyValuePair<string, string> cItem in cDictNew)
                {
                    cNew.Add($"{cItem.Key.Replace("_", "")} {cItem.Value}");
                }

                List<string> cOnlyLast = cLast.Except(cNew).ToList();
                List<string> cOnlyNew = cNew.Except(cLast).ToList();

                //вывод удаленных
                foreach (string strItem in cOnlyLast)
                {
                    Console.WriteLine($"новый \"\"\tстарый \"{strItem}\" (удалено)");
                }

                //вывод новых
                foreach (string strItem in cOnlyNew)
                {
                    Console.WriteLine($"новый \"{strItem}\"\tстарый \"\" (добавлено)");
                }

                Console.ReadKey();
            }
            catch (Exception cException)
            {
                throw new Exception("Не удалось получить изменения в ЖУРНАЛЕ СОБЫТИЙ", cException);
            }

