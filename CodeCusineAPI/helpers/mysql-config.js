const mysql2 = require("mysql2");
require("dotenv").config(); // Load environment variables

const pool = mysql2.createPool({
  connectionLimit: 10,
  host: process.env.DBHOST,
  user: process.env.DBUSER,
  password: process.env.DBPASS,
  database: process.env.DBNAME,
  port: process.env.DBPORT,
});

pool.getConnection((err, connection) => {
  if (err) {
    console.error("Error connecting to the database:", err);
    throw err;
  }
  console.log("Connected to the database");
  connection.release(); // Release the connection
});

module.exports = pool;
