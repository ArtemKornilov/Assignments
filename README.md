Консольное приложение, выполняющее операции над матрицами: сложение, вычитание, умножение, транспонирование.
На вход приложение получает имядиректории с текстовыми файлами. Файлов может быть много.

Файл имеетследующий формат:
1я строка - слово означающее, что нужно сделать (multiply,add, subtract, transpose). 
Затем пустая строка. Далее матрицы(а), над которыми нужно провести операцию. 
Числа в строке матрицы разделены пробелами, матрицы друг от друго отделены пустой строкой.

Пример содержимого файла:


transpose

2 5 6 99

8 55 6 9

7 8 5 56



В результате умножения, сложения, вычитания должна получиться 1 матрица. В результате транспонирования - столько же сколько и во входном файле.
В результате работы программа должна сохранить в исходную папку файлы с ответами, по одному на каждый входной файл (<имяиходногофайла>_result.txt).
Программа должна отображать текущийпрогресс.
