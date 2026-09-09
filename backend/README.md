Backend — Transmar Recruitment (Day 1)

This is a minimal Node.js + TypeScript backend scaffold using Express and Prisma (SQLite) created as part of Day 1 tasks.

Quick start

1. Install dependencies

   cd backend
   npm install

2. Generate Prisma client and migrate database

   # copy env example
   cp .env.example .env

   # generate client
   npx prisma generate

   # apply migrations (creates SQLite file)
   npx prisma migrate dev --name init

3. (Optional) Seed sample data

   npm run seed

4. Run dev server

   npm run dev

API Endpoints (basic)

- GET /health
- Products: /api/products
- Assembly lines: /api/assembly-lines

Notes
- Database is configured to use SQLite by default (see `.env.example`). Change `DATABASE_URL` to use PostgreSQL in production.
- This is a minimal scaffold that implements Product and AssemblyLine basic CRUD and a seed script.

Next steps
- Add Workstation model and allocations
- Add authentication
- Add tests

