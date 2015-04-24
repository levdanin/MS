var html = SHJSTerm.sendCommand(SHJSTerm.COMMAND_HTTP_GET, {url:"https://www.google.com"}).data;
SHJSTerm.sendCommand(SHJSTerm.COMMAND_OUTPUT, {data:html});