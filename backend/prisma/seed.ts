import { PrismaClient } from '@prisma/client';
const prisma = new PrismaClient();

async function main(){
  // Clean up in dependency order
  await prisma.assemblyLineWorkstation.deleteMany();
  await prisma.workstation.deleteMany();
  await prisma.assemblyLine.deleteMany();
  await prisma.product.deleteMany();

  const p1 = await prisma.product.create({ data: { name: '8DAB' } });
  const p2 = await prisma.product.create({ data: { name: '8DJH' } });
  const p3 = await prisma.product.create({ data: { name: 'Simosec' } });
  const p4 = await prisma.product.create({ data: { name: 'NXPlus C' } });

  const a1 = await prisma.assemblyLine.create({ data: { name: 'Convey line', active: true, productId: p1.id } });
  const a2 = await prisma.assemblyLine.create({ data: { name: 'Manual line', active: true, productId: p2.id } });
  const a3 = await prisma.assemblyLine.create({ data: { name: 'Final assembly line', active: true, productId: p3.id } });
  const a4 = await prisma.assemblyLine.create({ data: { name: 'Testing line', active: true, productId: p4.id } });

  const workstationsData = [
    { short_name: 'Laser', name: 'Laser welding', pc_name: 'pc-laser' },
    { short_name: 'ManualWeld', name: 'Manual welding', pc_name: 'pc-manual' },
    { short_name: 'Drive', name: 'Drive assembly', pc_name: 'pc-drive' },
    { short_name: 'Volt', name: 'Voltage drop test', pc_name: 'pc-volt' },
    { short_name: 'Leak', name: 'Leakage test', pc_name: 'pc-leak' },
    { short_name: 'HVPD', name: 'HV/PD test', pc_name: 'pc-hvpd' },
    { short_name: 'Final', name: 'Final inspection', pc_name: 'pc-final' },
    { short_name: 'Frame', name: 'Frame assembly', pc_name: 'pc-frame' },
    { short_name: 'Testing', name: 'Testing', pc_name: 'pc-testing' },
    { short_name: 'Dispatch', name: 'Dispatch', pc_name: 'pc-dispatch' }
  ];

  const createdWorkstations = [] as any[];
  for (const w of workstationsData) {
    const created = await prisma.workstation.create({ data: w });
    createdWorkstations.push(created);
  }

  // Create some sample allocations (preserve order by position)
  await prisma.assemblyLineWorkstation.createMany({
    data: [
      { assemblyLineId: a1.id, workstationId: createdWorkstations[0].id, position: 1 },
      { assemblyLineId: a1.id, workstationId: createdWorkstations[1].id, position: 2 },
      { assemblyLineId: a1.id, workstationId: createdWorkstations[2].id, position: 3 },

      { assemblyLineId: a2.id, workstationId: createdWorkstations[1].id, position: 1 },
      { assemblyLineId: a2.id, workstationId: createdWorkstations[3].id, position: 2 },

      { assemblyLineId: a3.id, workstationId: createdWorkstations[6].id, position: 1 },
      { assemblyLineId: a3.id, workstationId: createdWorkstations[7].id, position: 2 }
    ]
  });

  console.log('Seeded sample products, assembly lines, workstations and allocations');
}

main()
  .catch((e) => {
    console.error(e);
    process.exit(1);
  })
  .finally(async () => {
    await prisma.$disconnect();
  });
