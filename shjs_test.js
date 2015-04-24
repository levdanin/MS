var html = SHJSTerm.sendCommand(SHJSTerm.COMMAND_HTTP_REQUEST, {url:"https://www.google.com"}).data;
SHJSTerm.sendCommand(SHJSTerm.COMMAND_OUTPUT, {data:html});