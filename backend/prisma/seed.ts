import { PrismaClient } from '@prisma/client';
const prisma = new PrismaClient();

async function main(){
  await prisma.assemblyLine.deleteMany();
  await prisma.product.deleteMany();

  const p1 = await prisma.product.create({ data: { name: '8DAB' } });
  const p2 = await prisma.product.create({ data: { name: '8DJH' } });
  const p3 = await prisma.product.create({ data: { name: 'Simosec' } });
  const p4 = await prisma.product.create({ data: { name: 'NXPlus C' } });

  await prisma.assemblyLine.createMany({
    data: [
      { name: 'Convey line', active: true, productId: p1.id },
      { name: 'Manual line', active: true, productId: p2.id },
      { name: 'Final assembly line', active: true, productId: p3.id },
      { name: 'Testing line', active: true, productId: p4.id }
    ]
  });

  console.log('Seeded sample products and assembly lines');
}

main()
  .catch((e) => {
    console.error(e);
    process.exit(1);
  })
  .finally(async () => {
    await prisma.$disconnect();
  });
