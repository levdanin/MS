var SHJSTerm = {
    
    COMMAND_OUTPUT : 'OUTPUT',
	COMMAND_LOG : 'LOG',
    COMMAND_WRITE_FILE : 'WRITE_FILE',
	COMMAND_WRITE_TMP_FILE : 'WRITE_TMP_FILE',
	COMMAND_DELETE_FILE : 'DELETE_FILE',
	COMMAND_READ_FILE : 'READ_FILE',
	COMMAND_SLEEP : 'SLEEP',
	COMMAND_HTTP_REQUEST : 'HTTP_REQUEST',
    COMMAND_HTTP_GET : 'HTTP_GET',
    COMMAND_HTTP_POST : 'HTTP_POST',
    COMMAND_SET_COOKIES : 'SET_COOKIES',
    COMMAND_GET_COOKIES : 'GET_COOKIES',
    DELIMITER_OUTPUT : '<<<',
    DELIMITER_INPUT : '>>>',
    RESULT_OK : 'OK',
    RESULT_ERROR : 'ERROR',
    
    sendCommand: function(command, options)
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
    }
}

