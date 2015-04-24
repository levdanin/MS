//document.body.innerHTML = '<div class="g-recaptcha" data-sitekey="6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3" data-type="image"></div>';

var jqs = document.createElement('script');
jqs.type = 'text/javascript';
jqs.src = 'https://www.google.com/recaptcha/api.js';
document.getElementsByTagName('head')[0].appendChild(jqs);

document.body.innerHTML += '<div id="g-recaptcha-div"></div>';
document.head.innerHTML += '<script type="text/javascript">var onloadCallback = function() {grecaptcha.render("g-recaptcha-div", {"sitekey" : "6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3"});};</script>';


SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:document.innerHTML});

SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"The END"});