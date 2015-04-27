var SHJSTerm = function(command, options) {
    return SHJSTerm.sendCommand(command, options);
}

SHJSTerm.COMMAND_OUTPUT = 'OUTPUT';
SHJSTerm.COMMAND_LOG = 'LOG';
SHJSTerm.COMMAND_WRITE_FILE = 'WRITE_FILE';
SHJSTerm.COMMAND_WRITE_TMP_FILE = 'WRITE_TMP_FILE';
SHJSTerm.COMMAND_DELETE_FILE = 'DELETE_FILE';
SHJSTerm.COMMAND_READ_FILE = 'READ_FILE';
SHJSTerm.COMMAND_SLEEP = 'SLEEP';
SHJSTerm.COMMAND_HTTP_REQUEST = 'HTTP_REQUEST';
SHJSTerm.COMMAND_HTTP_GET = 'HTTP_GET';
SHJSTerm.COMMAND_HTTP_POST = 'HTTP_POST';
SHJSTerm.COMMAND_SET_COOKIES = 'SET_COOKIES';
SHJSTerm.COMMAND_GET_COOKIES = 'GET_COOKIES';
SHJSTerm.DELIMITER_OUTPUT = '<<<';
SHJSTerm.DELIMITER_INPUT = '>>>';
SHJSTerm.RESULT_OK = 'OK';
SHJSTerm.RESULT_ERROR = 'ERROR';
SHJSTerm.RESULT_TERMINATE = 'TERMINATE';

SHJSTerm.sendCommand = function(command, options)
{
    var sendText = this.DELIMITER_OUTPUT + command + this.DELIMITER_OUTPUT + JSON.stringify(options);
    print(sendText);

    var readText = '', readLine = readline();

    while((readLine !== this.DELIMITER_INPUT + this.RESULT_OK) && (readLine.indexOf(this.DELIMITER_INPUT + this.RESULT_ERROR + this.DELIMITER_INPUT) !== 0))
    {
        readText += readLine;
        readLine = readline();
    }
    if (readLine === this.DELIMITER_INPUT + this.RESULT_OK)
    {
        return {status: 1, data: readText};
    }
    else if (readLine.indexOf(this.DELIMITER_INPUT + this.RESULT_ERROR + this.DELIMITER_INPUT) === 0)
    {
        return {status: -1, message: readLine.substring((this.DELIMITER_INPUT + this.RESULT_ERROR + this.DELIMITER_INPUT).length), data: readText};
    }        
    else if (readLine.indexOf(this.DELIMITER_INPUT + this.RESULT_TERMINATE + this.DELIMITER_INPUT) === 0)
    {
        print("Latest read text:");
        print(readText);
        var msg = readLine.substring((this.DELIMITER_INPUT + this.RESULT_TERMINATE + this.DELIMITER_INPUT).length);
        print("********** ABORTED: " + msg + " **********");
        throw new Error(msg);
    }        
}
