## Markdown specification
    ## Markdown specification

### Format text
	### Format text

You can use __underscores__ or **asterics** for highlight text with strong tag.  
	You can use __underscores__ or **asterics** for highlight text with strong tag.

You _can write __nested__ constructions_ with this modificators.  
	You _can write __nested__ constructions_ with this modificators.

You also can use `backtics for insert __text__ _without_ formatting`.  
	You also can use `backtics for insert __text__ _without_ formatting`.

If you want use strong and emphasis modificator simultaneously you can write __*something*__ **_like_** *__this__*.  
	If you want use strong and emphasis modificator simultaneously you can write __*something*__ **_like_** *__this__*.

Use modificators careful. You can't surround it with punctuation signs or spaces. It won't parsed **!in this case!**.  
	Use modificators careful. You can't surround it with punctuation signs or spaces. It won't parsed **!in this case!**.

### Paragraphs
	### Paragraphs

You can simply break paragraph with two spaces "  " at the end of line.  
	You can simply break paragraph with two spaces "  " at the end of line.

You can also break paragraph with two line breaks.

### Code blocks
	### Code blocks

Just indent your code with four spaces or single tab and it will be well formated!

	def hypot(a, b):
		return math.sqrt(a**2 + b**2)

		def hypot(a, b):
			return math.sqrt(a**2 + b**2)

### Links
	### Links

You can simply include [links](https://daringfireball.net/projects/markdown/syntax) into your text.  
	You can simply include [links](https://daringfireball.net/projects/markdown/syntax) into your text.

You allowed to insert as many spaces as you need between link text and link reference.

Unfortunately, you can't use format modificators in [your *link* text](http://sor.ry).  
	Unfortunately, you can't use format modificators in [your *link* text](http://sor.ry).

### Configuration
	### Configuration

Simple way is to use command line interface arguments. For example: `MarkdownCli.exe -i Spec.md -o Spec.html`  
	Simple way is to use command line interface arguments. For example: `MarkdownCli.exe -i Spec.md -o Spec.html`

Also you can pass configuration file as parameter and set some parameters from this file:
`MarkdownCli.exe --config config.yml`  
	Also you can pass configuration file as parameter and set some parameters from this file: `MarkdownCli.exe --config config.yml`

Configuration file must be written in [YAML](https://en.wikipedia.org/wiki/YAML) format.  
	Configuration file must be written in [YAML](https://en.wikipedia.org/wiki/YAML) format.

Your fields in YAML file must has same name with CLI arguments.  
	input: Spec.md
	output: Spec.html
	html_file: template.html
	inject_element: "#markdown"
	class: markdown_el

Configuration file parameters **do not override** parameters passed as command lina arguments.  
	Configuration file parameters **do not override** parameters passed as command lina arguments.