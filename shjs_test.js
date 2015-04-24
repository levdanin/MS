document.body.innerHTML = '<div class="g-recaptcha" data-sitekey="6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3"></div>';

var jqs = document.createElement('script');
jqs.type = 'text/javascript';
jqs.src = 'https://www.google.com/recaptcha/api.js';
document.getElementsByTagName('head')[0].appendChild(jqs);

SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:document.innerHTML});