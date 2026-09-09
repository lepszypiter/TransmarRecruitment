import { Request, Response } from 'express';
import prisma from '../prismaClient';

export const listAllocations = async (req: Request, res: Response) => {
  const assemblyLineId = req.params.id;
  const allocations = await prisma.assemblyLineWorkstation.findMany({
    where: { assemblyLineId },
    include: { workstation: true },
    orderBy: { position: 'asc' }
  });
  res.json(allocations);
};

export const createAllocations = async (req: Request, res: Response) => {
  const assemblyLineId = req.params.id;
  const { workstationIds } = req.body as { workstationIds?: string[] | string };
  if (!workstationIds) return res.status(400).json({ message: 'workstationIds required' });

  const ids = Array.isArray(workstationIds) ? workstationIds : [workstationIds];

  // compute current max position
  const currentMax = await prisma.assemblyLineWorkstation.findFirst({
    where: { assemblyLineId },
    orderBy: { position: 'desc' }
  });
  let position = currentMax ? currentMax.position + 1 : 1;

  const created: any[] = [];
  for (const wid of ids) {
    try {
      const record = await prisma.assemblyLineWorkstation.create({ data: { assemblyLineId, workstationId: wid, position } });
      created.push(record);
      position += 1;
    } catch (err: any) {
      // skip duplicates or errors per item
    }
  }

  res.status(201).json(created);
};

export const reorderAllocations = async (req: Request, res: Response) => {
  const assemblyLineId = req.params.id;
  const { order } = req.body as { order: string[] };
  if (!Array.isArray(order)) return res.status(400).json({ message: 'order must be array of allocation ids' });

  const tx = order.map((allocationId, index) =>
    prisma.assemblyLineWorkstation.update({ where: { id: allocationId }, data: { position: index + 1 } })
  );

  try {
    const updated = await prisma.$transaction(tx);
    res.json(updated);
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};

export const deleteAllocation = async (req: Request, res: Response) => {
  const { id: assemblyLineId, allocationId } = req.params as { id: string; allocationId: string };
  try {
    await prisma.assemblyLineWorkstation.delete({ where: { id: allocationId } });
    res.status(204).send();
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};
