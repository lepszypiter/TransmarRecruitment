import { Request, Response } from 'express';
import prisma from '../prismaClient';

export const listWorkstations = async (req: Request, res: Response) => {
  const workstations = await prisma.workstation.findMany({ orderBy: { name: 'asc' } });
  res.json(workstations);
};

export const getWorkstation = async (req: Request, res: Response) => {
  const { id } = req.params;
  const workstation = await prisma.workstation.findUnique({ where: { id } });
  if (!workstation) return res.status(404).json({ message: 'Workstation not found' });
  res.json(workstation);
};

export const createWorkstation = async (req: Request, res: Response) => {
  const { short_name, name, pc_name } = req.body;
  if (!name || !short_name) return res.status(400).json({ message: 'short_name and name are required' });
  try {
    const workstation = await prisma.workstation.create({ data: { short_name, name, pc_name } });
    res.status(201).json(workstation);
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};

export const updateWorkstation = async (req: Request, res: Response) => {
  const { id } = req.params;
  const { short_name, name, pc_name } = req.body;
  try {
    const workstation = await prisma.workstation.update({ where: { id }, data: { short_name, name, pc_name } });
    res.json(workstation);
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};

export const deleteWorkstation = async (req: Request, res: Response) => {
  const { id } = req.params;
  try {
    await prisma.workstation.delete({ where: { id } });
    res.status(204).send();
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};
