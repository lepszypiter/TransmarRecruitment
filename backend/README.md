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

Tests

Run unit tests (Jest + ts-jest):

  npm install
  npm test

Deploy notes

- The app uses SQLite by default for local development (`DATABASE_URL` in `.env`). For production use PostgreSQL or another provider — update `prisma/schema.prisma` datasource and migrate.
- Build backend: `npm run build` then `npm start` (ensure `NODE_ENV=production` and `DATABASE_URL` point to production DB).
- For containerized deployment, create a `Dockerfile` and `docker-compose.yml` (not included) to run Postgres and the Node app. Expose port defined by `PORT` env var (default 4000).
- Secure secrets: set `JWT_SECRET` and do not commit `.env`.

Final cleanup

- Run `npx prisma generate` after changing schema to update client.
- Remove dev-only seed data before sharing production credentials.


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

