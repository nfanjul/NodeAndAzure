// Execute, in console --> node sql.js 

var sql = require('mssql');

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

function loadProducts() {
    sql.connect(config, function (err) {
        if (err) console.log(err);
        var request = new sql.Request();
        request.query('SELECT * FROM [SalesLT].[ProductCategory] pc JOIN [SalesLT].[Product] p ON pc.productcategoryid = p.productcategoryid',
            function (err, recordset) {
                if (err) console.log(err);
                console.log(recordset);
                sql.close();
            });
    });
}

loadProducts();
