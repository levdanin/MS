try 
{
    //document.body.innerHTML = '<div class="g-recaptcha" data-sitekey="6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3" data-type="image"></div>';

//document.location = "http://www.bookjetty.com/signup";

/*
    document.body.innerHTML = '<div id="g-recaptcha-div"></div><script type="text/javascript">grecaptcha.render(\'g-recaptcha-div\', {sitekey : \'6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3\'});</script>';

    var jqs = document.createElement('script');
    jqs.type = 'text/javascript';
    jqs.src = 'http://code.jquery.com/jquery.min.js';
    document.getElementsByTagName('head')[0].appendChild(jqs);


    var jqs1 = document.createElement('script');
    jqs1.type = 'text/javascript';
    jqs1.src = 'https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit';
    document.getElementsByTagName('head')[0].appendChild(jqs1);


    var jqs3 = document.createElement('script');
    jqs3.type = 'text/javascript';
    jqs3.text = "SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:'jQuery onload from body'});";
    document.getElementsByTagName('body')[0].appendChild(jqs3);
    

    document.getElementsByTagName('body')[0].onload = "SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:'BODY onload'})";

*/

    document.head.innerHTML = "";
    document.body.innerHTML = "<div></div>";

    var jqs0 = document.createElement('script');
    jqs0.type = 'text/javascript';
    jqs0.text = 'SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {"data":"HEAD script DOM 1"})';
    document.getElementsByTagName('head')[0].appendChild(jqs0);

    var jqs01 = document.createElement('script');
    jqs01.type = 'text/javascript';
    jqs01.text = 'SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {"data":"HEAD script DOM 1_1"})';
    document.getElementsByTagName('head')[0].appendChild(jqs01);


    document.head.innerHTML += '<script type="text/javascript">SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"HEAD script innerHTML 1"})</script>'
                              +'<script type="text/javascript">SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"HEAD script innerHTML 2"})</script>';

    var jqs2 = document.createElement('script');
    jqs2.type = 'text/javascript';
    jqs2.text = 'SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"HEAD script DOM 2"})';
    document.getElementsByTagName('head')[0].appendChild(jqs2);

    var jqs3 = document.createElement('script');
    jqs3.type = 'text/javascript';
    jqs3.text = 'SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"BODY script DOM 1"})';
    document.getElementsByTagName('body')[0].appendChild(jqs3);

    var jqs31 = document.createElement('script');
    jqs31.type = 'text/javascript';
    jqs31.text = 'SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"BODY script DOM 1_1"})';
    document.getElementsByTagName('body')[0].appendChild(jqs31);

    document.body.innerHTML += '<script type="text/javascript">SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"BODY script innerHTML 1"})</script>'
                              +'<div id="maindiv1"></div>'
                              +'<script type="text/javascript">SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"BODY script innerHTML 2"})</script>';

    var jqs4 = document.createElement('script');
    jqs4.type = 'text/javascript';
    jqs4.text = 'SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"BODY script DOM 2"})';
    document.getElementsByTagName('body')[0].appendChild(jqs4);


    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:document.innerHTML});

    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"The END"});
}
catch (e)
{
    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"Error during test: " + e.toString()});
}