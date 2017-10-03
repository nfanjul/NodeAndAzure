var express = require('express');
var bodyParser = require('body-parser');
var http = require('http')
var fs = require('fs');
var azure = require('azure-storage');
var redis = require('redis');
var multer = require('multer');
var upload = multer({ limits: { fileSize: 2000000 }, dest: '/home/site/wwwroot/uploads/' });
var sql = require('mssql');

var port = process.env.PORT || 4000;
var app = express();
var api = express.Router();

// Example call simple
api.get('/', function (req, res) {
    var date = new Date();
    res.send('Hello World!! ' + TimeNow(date));
});

// Example add file from azuure storage
api.post('/addblob', upload.single('fileToUpload'), function (req, res) {
    var date = new Date();
    var blobService = azure.createBlobService('DefaultEndpointsProtocol=https;AccountName=<AccountName>;AccountKey=<AccountKey>;EndpointSuffix=core.windows.net');
    var container = '<ContainerName>';
    var blob = TimeNow(date) + '.jpg';
    blobService.createContainerIfNotExists(container, error => {
        if (error) return console.log(error);
        blobService.createBlockBlobFromLocalFile(
            container,
            blob,
            req.file.path,
            (error, result) => {
                if (error) {
                    res.send(error);
                }
                else {
                    res.send(result);
                }
            }
        );
    });
});

// Example Set value in redis cache
api.post('/setredis', function (req, res) {
    var client = redis.createClient(6380, '<redisHost>', {
        auth_pass: '<redisKey>',
        tls: { servername: '<redisHost>' }
    });
    var bodyStr = '';
    req.on('data', function (chunk) {
        bodyStr += chunk.toString();
    });
    req.on('end', function () {
        client.set('key', bodyStr, (error, reply) => {
            if (error) {
                res.send(error);
            }
            else {
                res.send(reply);
            }
        });
    });
});

// Example Get value from redis cache
api.get('/getredis', function (req, res) {
    var client = redis.createClient(6380, '<redisHost>', {
        auth_pass: '<redisKey>',
        tls: { servername: '<redisHost>' }
    });
    client.get('key', (error, reply) => {
        if (error) {
            return res.send(error);
        }
        else {
            return res.send(reply);
        }
    });
});

// Example select from sql
api.get('/getsql', function (req, res) {
    var config = {
        user: '<user>',
        password: '<pass>',
        server: '<server>',
        database: '<database>',
        pool: {
            max: 10,
            min: 0,
            idleTimeoutMillis: 30000,
        },
        options:
        {
            encrypt: true,
        }
    }
    sql.connect(config, function (err) {
        if (err) console.log(err);
        var request = new sql.Request();
        request.query('SELECT pc.Name as CategoryName, p.name as ProductName FROM [SalesLT].[ProductCategory] pc JOIN [SalesLT].[Product] p ON pc.productcategoryid = p.productcategoryid', function (err, recordset) {
            if (err) console.log(err);
            res.send(recordset);
            sql.close();
        });
    });
});

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: true
}));
app.use('/api', api);

var server = http.createServer(app);

server.listen(port);

function TimeNow(date) {
    return date.getDate() + '_' + (date.getMonth()) + '_' + date.getFullYear() + '-' + date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
}