Option Explicit



'Действие перед построением отчета
Public Sub PreBuildAction()
    SetDate
End Sub



'Действие после построением отчета
Public Sub PostBuildAction()
    
End Sub



'Метод инициализирует колонку дат
Public Sub SetDate()

    Dim nCountDays As Integer
    
    nCountDays = GetDaysInMonth(Application.ThisWorkbook.Sheets("Лист1").Cells(2, 2))

End Sub



'Получить дней в месяце
Public Function GetDaysInMonth(dDate As Date) As Integer

    'Нужно определить первое число следующего месяца и от этой даты отнять один день, затем получить число - это и будет количество дней в месяце
    Dim nMonth As Integer                   'Порядковый номер Месяца
    Dim nYear As Integer                    'Порядковый номер Года
    Dim dDateFirstDayInMonth As Date        'Дата первого дня месяца
    Dim dDateFirstDayInNextMonth As Date    'Дата первого дня следующего месяца
    Dim dDateLastDayInMonth As Date         'Дата последнего дня месяца
    
        
    nMonth = Month(dDate)                                               'месяц
    nYear = Year(dDate)                                                 'год
    dDateFirstDayInMonth = DateSerial(nYear, nMonth, 1)                 'получаем дату первого дня месяца
    dDateFirstDayInNextMonth = DateAdd("m", 1, dDateFirstDayInMonth)    'получаем дату первого дня в следующем месяце
    dDateLastDayInMonth = DateAdd("d", -1, dDateFirstDayInNextMonth)    'получаем дату последнего дня месяца
    
    GetDaysInMonth = Day(dDateLastDayInMonth)
    
End Function
