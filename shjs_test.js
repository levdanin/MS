try 
{
    //document.body.innerHTML = '<div class="g-recaptcha" data-sitekey="6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3" data-type="image"></div>';

    /*
    document.body.innerHTML = '';

    document.location = "http://www.bookjetty.com/signup";
    */

    document.body.innerHTML = '';

    var jqs1 = document.createElement('script');
    jqs1.type = 'text/javascript';
    jqs1.src = 'https://www.google.com/recaptcha/api.js';//?onload=onloadCallback&render=explicit';
    document.getElementsByTagName('head')[0].appendChild(jqs1);

    document.body.innerHTML = '<div id="g-recaptcha-div"></div>'
                    //        + '<div class="g-recaptcha" data-sitekey="6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3" data-type="image"></div>'
                    ;

    var jqs3 = document.createElement('script');
    jqs3.type = 'text/javascript';
    jqs3.text = "grecaptcha.render(\'g-recaptcha-div\', {sitekey : \'6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3\'});";
    document.getElementsByTagName('head')[0].appendChild(jqs3);
    
    document.body.onload = function(){console.log('body onload')};
    

/*

    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"The START 0021"});

    document.head.innerHTML = "";
    document.body.innerHTML = "<div></div>";

    var jqs0 = document.createElement('script');
    jqs0.type = 'text/javascript';
    jqs0.text = 'console.log("HEAD script DOM 1")';
    document.getElementsByTagName('head')[0].appendChild(jqs0);

    var jqs01 = document.createElement('script');
    jqs01.type = 'text/javascript';
    jqs01.text = 'console.log("HEAD script DOM 1_1")';
    document.getElementsByTagName('head')[0].appendChild(jqs01);

    document.head.innerHTML += '<script type="text/javascript">console.log("HEAD script innerHTML 1")</script>'
                              +'<script type="text/javascript">console.log("HEAD script innerHTML 2")</script>';

    var jqs2 = document.createElement('script');
    jqs2.type = 'text/javascript';
    jqs2.text = 'console.log("HEAD script DOM 2")';
    document.getElementsByTagName('head')[0].appendChild(jqs2);

//    document.parsing = true;



    var jqs3 = document.createElement('script');
    jqs3.type = 'text/javascript';
    jqs3.text = 'console.log("BODY script DOM 1")';
    document.getElementsByTagName('body')[0].appendChild(jqs3);

    var jqs31 = document.createElement('script');
    jqs31.type = 'text/javascript';
    jqs31.text = 'console.log("BODY script DOM 1_1")';
    document.getElementsByTagName('body')[0].appendChild(jqs31);

    document.body.innerHTML += '<script type="text/javascript">console.log("BODY script innerHTML 1")</script>'
                              +'<div id="maindiv1"></div>'
                              +'<script type="text/javascript">console.log("BODY script innerHTML 2")</script>';

    var jqs4 = document.createElement('script');
    jqs4.type = 'text/javascript';
    jqs4.text = 'console.log("BODY script DOM 2")';
    document.getElementsByTagName('body')[0].appendChild(jqs4);


*/

    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"document.innerHTML:" + document.innerHTML});
    
    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"BODY.outerHTML:" + document.body.outerHTML});

    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"The END"});
}
catch (e)
{
    SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:"Error during test: " + e.toString()});
}