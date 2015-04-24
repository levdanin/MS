var resp = SHJSTerm(SHJSTerm.COMMAND_HTTP_REQUEST, {xhr:{url:"https://www.google.com", method: "GET"}});
SHJSTerm(SHJSTerm.COMMAND_OUTPUT, {data:JSON.stringify(resp)});