Требуется написать консольную программу на C# для генерации сигнатуры указанного файла. 
Сигнатура генерируется следующим образом: исходный файл делится на блоки заданной длины (кроме последнего блока), 
для каждого блока вычисляется значение hash-функции SHA256, и вместе с его номером выводится в консоль.  

Программа должна уметь обрабатывать файлы, размер которых превышает объем оперативной памяти, 
и при этом максимально эффективно использовать вычислительные мощности многопроцессорной системы. 
При работе с потоками допускается использовать только стандартные классы и библиотеки из .Net 3.5 
(исключая ThreadPool, BackgroundWorker, TPL). Путь до входного файла и размер блока задаются в командной строке.  
В случае возникновения ошибки во время выполнения программы ее текст и StackTrace необходимо вывести в консоль.
