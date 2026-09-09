Recruitment Challenge: Full Stack Node.js & Angular — Implementation Plan

Goal

Create a small assembly-line manager with two projects:
- Backend: RESTful API in Node.js + TypeScript
- Frontend: Angular application

High-level requirements (summary)
- Entities: `Product`, `AssemblyLine`, `Workstation`
- Relations: Product 1..* AssemblyLine; AssemblyLine *..* Workstation (allocation with order)
- Use cases: Login, manage products, assembly lines (CRUD + filter by product), workstations, allocations (add, remove, preserve and reorder)
- Authentication: either GitHub OAuth or local JWT-based login
- Persisted DB schema; ability to recreate schema + optional seeding
- Deliver: GitHub repo(s) + `README` with setup steps

Tech stack
- Backend: Node.js 18+/20+, TypeScript, Express (or NestJS if preferred), Prisma (or TypeORM) for DB ORM, PostgreSQL (primary) and SQLite for local quick dev. JWT for local auth; optional GitHub OAuth.
- Frontend: Angular (latest), Angular Router, Angular Material for UI components, RxJS for data flows.
- Dev tooling: ESLint, Prettier, Husky (optional), Docker (optional but recommended for DB)

Repository layout
- `backend/` — Node + TypeScript API
  - `src/` — `controllers`, `services`, `routes`, `prisma` (or `entities` + `migrations`)
  - `src/auth/` — JWT + (optional) OAuth handlers
  - `prisma/` — schema and seed scripts (if Prisma used)
  - `Dockerfile`, `docker-compose.yml` (for Postgres)
  - `README.md` (backend setup + migrate/seed commands)
- `frontend/` — Angular app
  - `src/app/` — modules: `auth`, `products`, `assembly-lines`, `workstations`, `allocations`, `shared`
  - `environment` files with API URL config
  - `README.md` (frontend setup + run instructions)
- Root `README.md` — overall project setup and how to run both apps

Data model (DB tables)
- `Product`:
  - `id` (uuid)
  - `name` (string, unique)
  - timestamps
- `AssemblyLine`:
  - `id` (uuid)
  - `name` (string)
  - `active` (boolean)
  - `productId` (fk -> Product.id)
  - timestamps
- `Workstation`:
  - `id` (uuid)
  - `short_name` (string)
  - `name` (string)
  - `pc_name` (string)
  - timestamps
- `AssemblyLineWorkstation` (allocation, preserves order):
  - `id` (uuid)
  - `assemblyLineId` (fk)
  - `workstationId` (fk)
  - `position` (integer) — lower = earlier in assembly line
  - timestamps

Backend API (suggested endpoints)
- Auth
  - `POST /api/auth/login` — local email/password => JWT
  - `GET /api/auth/github` & callback (if OAuth)
- Products
  - `GET /api/products` — list
  - `POST /api/products` — create
  - `GET /api/products/:id` — details
  - `PUT /api/products/:id` — update
  - `DELETE /api/products/:id` — delete
- Assembly lines
  - `GET /api/assembly-lines` — supports filter `?productId=` and pagination
  - `POST /api/assembly-lines`
  - `GET /api/assembly-lines/:id`
  - `PUT /api/assembly-lines/:id`
  - `DELETE /api/assembly-lines/:id`
- Workstations
  - `GET /api/workstations`
  - `POST /api/workstations`
  - `GET /api/workstations/:id`
  - `PUT /api/workstations/:id`
  - `DELETE /api/workstations/:id`
- Allocations
  - `GET /api/assembly-lines/:id/workstations` — list allocations (ordered)
  - `POST /api/assembly-lines/:id/workstations` — allocate (accepts workstationId or array; append to end or at position)
  - `PUT /api/assembly-lines/:id/workstations/reorder` — accept array of allocation ids or workstation ids to set `position`
  - `DELETE /api/assembly-lines/:id/workstations/:allocationId` — remove allocation

Authentication & Authorization
- Minimum: JWT token issued at `/auth/login` stored in browser `localStorage` and attached as `Authorization: Bearer <token>` in API requests.
- Protect all modifying routes with middleware to validate JWT.
- Option: Implement GitHub OAuth and allow login via GitHub (recommended for a demo).

Frontend app flow
- Public: login page (or OAuth redirect)
- After login: dashboard listing products and assembly lines
- Pages/components:
  - `Products` CRUD page + small form
  - `AssemblyLines` CRUD page with product filter and link to allocations
  - `Workstations` CRUD page
  - `Allocation` page for a given assembly line: shows ordered list of allocated workstations with drag-and-drop reordering (Angular CDK DragDrop), ability to add existing workstations (multi-select) and remove allocations
- Use Angular services to call backend endpoints and centralize error handling

Seeding & sample data
- Provide a `prisma/seed.ts` or `scripts/seed.js` that creates sample data:
  - Products: `8DAB`, `8DJH`, `Simosec`, `NXPlus C`
  - Assembly lines: `Convey line`, `Manual line`, `Final assembly line`, `Testing line` (assign to products)
  - Workstations: `Laser welding`, `Manual welding`, `Drive assembly`, `Voltage drop test`, `Leakage test`, `HV/PD test`, `Final inspection`, `Frame assembly`, `Testing`, `Dispatch`
- Provide SQL or ORM seed script and document in README how to run it

Setup & run (developer experience)
- Backend:
  - `npm install`
  - provide `.env.example` with `DATABASE_URL`, `JWT_SECRET`, `GITHUB_CLIENT_ID/SECRET` (if used)
  - `npx prisma migrate dev --name init` or equivalent
  - `npm run seed` (optional)
  - `npm start` or `npm run dev`
- Frontend:
  - `npm install`
  - update `environment.dev.ts` to point to backend API
  - `ng serve`
- Docker (optional): `docker-compose up` to run Postgres + backend or full stack

Testing
- Backend: unit tests for services + integration test for endpoints (supertest) and a smoke test for auth
- Frontend: basic unit tests for services and critical components; e2e tests optional (Cypress)

Deliverables checklist
- [ ] `backend/` + `frontend/` projects in a single repository or two repositories
- [ ] Root and project READMEs with setup and run instructions
- [ ] DB schema + migrations + optional seeding script
- [ ] Authentication (JWT or GitHub OAuth)
- [ ] CRUD for products, assembly lines, workstations
- [ ] Allocation management with order and reorder support
- [ ] Sample data available to test
- [ ] Invite reviewer `cezary-z` as GitHub collaborator

Suggested implementation timeline (solo developer)
- Day 1: Repo layout, backend basic API (Products + Assembly lines basic CRUD), DB migrations
- Day 2: Workstations CRUD, allocation schema and APIs, seeding
- Day 3: Auth (JWT + optional OAuth), secure routes, simple frontend skeleton
- Day 4: Implement Angular pages, allocation UI (drag/drop), polishing and README
- Day 5: Tests, deploy notes, final cleanup, push to GitHub and invite reviewer

Notes / trade-offs
- Use Prisma + PostgreSQL for developer ergonomics and migrations; SQLite is acceptable for quick demo and CI
- OAuth is nice for demo but adds setup complexity; JWT + local credentials is faster
- Angular CDK DragDrop makes reordering simple on frontend; backend must accept position updates atomically

Next steps I will take on request
- Create the `backend` and `frontend` project scaffolding or
- Add detailed API spec (`openapi.yaml`) and DB schema files or
- Implement one slice (e.g., backend `Product` CRUD + migrations) and tests

