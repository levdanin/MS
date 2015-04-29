
//setTimeOut(function(){console.log("ok")}, 1000);

/*
var now = Date.now();


document.addEventListener('DOMContentLoaded', function(){
    console.log('%s (loaded in %s ms)', document.title, Date.now() - now);
}, true);

document.location = "http://www.bookjetty.com/signup";
*/

try 
{


    console.log("The START ---");

    //document.location = "http://www.bookjetty.com/signup";
    
    /*

    document.parsing = true;

    document.body.innerHTML = '';

    var jqs1 = document.createElement('script');
    jqs1.type = 'text/javascript';
    jqs1.src = 'https://www.google.com/recaptcha/api.js';
    document.getElementsByTagName('head')[0].appendChild(jqs1);

    document.body.innerHTML = '<div class="g-recaptcha" id="g-recaptcha-div" data-sitekey="6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3" data-type="image"></div>';
    
    var jqs3 = document.createElement('script');
    jqs3.type = 'text/javascript';
    jqs3.text = "grecaptcha.render(\'g-recaptcha-div\', {sitekey : \'6LfFYAATAAAAALgkU2LlG2cMgBh1wASqutIc-5e3\'});";
    document.getElementsByTagName('head')[0].appendChild(jqs3);
    */

    document.location = "http://www.diigo.com/sign-up";
    
    console.log("IFRAME count = " + document.getElementsByTagName('iframe').length);
    
    /*

    document.head.innerHTML = "";
    document.body.innerHTML = "<div></div>";

    document.getElementsByTagName('body')[0].setAttribute('onload', "console.log('body onload - attr');");
    document.getElementsByTagName('body')[0].onload = "console.log('body onload - =');";
 
 console.log("" + document.body.onload);
 console.log("" + document.body.getAttribute("onload"));

    console.log("document.innerHTML:" + document.innerHTML);
    
    console.log("HEAD.outerHTML:" + document.head.outerHTML);
    
    console.log("BODY.outerHTML:" + document.body.outerHTML);
    
    console.log("BODY.onload:" + document.body.onload());
    */

    console.log("document.innerHTML:" + document.innerHTML);
    console.log("The END");
    
}
catch (e)
{
    console.log("Error during test: " + e.toString());
}

