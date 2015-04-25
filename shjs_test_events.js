try 
{


    console.log("The START ---");

    document.head.innerHTML = "";
    document.body.innerHTML = "<div></div>";

    document.body.setAttribute('onload', "console.log('body onload0')");
 


    console.log("document.innerHTML:" + document.innerHTML);
    
    console.log("HEAD.outerHTML:" + document.head.outerHTML);
    
    console.log("BODY.outerHTML:" + document.body.outerHTML);
    
    console.log("BODY.onload:" + document.body.onload);

    console.log("The END");
    
}
catch (e)
{
    console.log("Error during test: " + e.toString());
}