// Execute, in console --> node redistest.js 

var redis = require('redis');

var client = redis.createClient(6380, '<redisHost>', {
    auth_pass: '<redisKey>',
    tls: { servername: '<redisHost>' }
});

client.set('key1', 'value', (err, reply) => {
    console.log(reply);
});

client.get('key1', (err, reply) => {
    console.log(reply);
});