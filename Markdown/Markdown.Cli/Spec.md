# Implemented Markdown syntax

You can simply _emphasize_ any string with __next__ available modificators: \_, \_\_, \*, \*\*.

`You can simply \_emphasize\_ any string with \_\_next\_\_ available modificators: \\\_, \\\_\\\_, \\\*, \\\*\\\*.`

If you do not want to apply modificators - simply escape it with \\ character!

You can use backticks (\`) for selecting blocks of code. For example:

`#include <iostream>
using namespace std;
int main()
{
	cout << "Hello, world!";
	return 0;
}`

There is a little surpirse - you can make your modificators nested!  
*You will get **something like this***

 __*It works*__

I don't want to disappoint you, but in this simple parser you can find many disadvantages.

I want enumerate some example of such bad cases:

1. You can't parse triple-modificators like this: `\*\*\*Be careful! Very angry dog!\*\*\*` 
But you can use next hack for this modificator: `\_\*\*It works!\*\*\_`

2. Your code must be surrounded with spaces. So, you can't write something like this:

Let me show my code:\`code\`

3. I can't find another curious behaviour but i'm sure - it is

You can paste links [like this](http://umqra.github.io/adaptive_design)

Also you can use [relative paths](/about.html) like this.

# Спецификация языка разметки

Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github.

Процессору на вход подается одна строка — параграф текста. 
На выходе должен быть HTML-код этого параграфа.

Текст _окруженный с двух сторон_  одинарными символами подчерка 
должен помещаться в HTML-тег em вот так:

`Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка 
должен помещаться в HTML-тег em вот так:`

Любой символ можно экранировать, чтобы он не считался частью разметки. 
\_Вот это\_, не должно выделиться тегом \<em\>.

__Двумя символами__ — должен становиться жирным с помощью тега \<strong\>.

Внутри __двойного выделения _одинарное_ тоже__ работает.

Но не наоборот — внутри _одинарного __двойное__ не работает_.

Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.

__непарные _символы не считаются выделением.

За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением 
и остаются просто символами подчерка.

Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения 
и остаются просто символами подчерка.
