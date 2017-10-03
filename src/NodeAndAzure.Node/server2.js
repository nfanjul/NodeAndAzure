// Example ES6

import express from 'express';
import bodyParser from 'body-parser';
import http from 'http';

const port = process.env.PORT || 4000;
const app = express();
const api = express.Router();

api.get('/', (req, res) => {
    res.send('Hello World!!');
});

app.use(bodyParser.json());
app.use('/api', api);

const server = http.createServer(app);

server.listen(port);