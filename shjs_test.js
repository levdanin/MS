try 
{
    //document.body.innerHTML = '<div class="g-recaptcha" data-sitekey="6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3" data-type="image"></div>';

    document.body.innerHTML = '<div id="g-recaptcha-div"></div>';

    var jqs = document.createElement('script');
    jqs.type = 'text/javascript';
    jqs.src = 'http://code.jquery.com/jquery.min.js';
    document.getElementsByTagName('head')[0].appendChild(jqs);


    var jqs1 = document.createElement('script');
    jqs1.type = 'text/javascript';
    jqs1.src = 'https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit';
    document.getElementsByTagName('head')[0].appendChild(jqs1);


    var jqs2 = document.createElement('script');
    jqs2.type = 'text/javascript';
    jqs2.text = "grecaptcha.render('g-recaptcha-div', {sitekey : '6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3'});";
    document.getElementsByTagName('head')[0].appendChild(jqs2);
    

    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:document.innerHTML});

    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"The END"});
}
catch (e)
{
    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"Error during test: " + e.toString()});
}