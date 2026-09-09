import app from './app';
import dotenv from 'dotenv';
import prisma from './prismaClient';

dotenv.config();

const port = process.env.PORT || 4000;

async function main(){
  // Test DB connection
  try {
    await prisma.$connect();
    console.log('Connected to database');
  } catch (err){
    console.error('Database connection error', err);
  }

  app.listen(port, () => {
    console.log(`Server listening on port ${port}`);
  });
}

main();
