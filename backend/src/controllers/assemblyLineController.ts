import { Request, Response } from 'express';
import prisma from '../prismaClient';

export const listAssemblyLines = async (req: Request, res: Response) => {
  const { productId } = req.query as { productId?: string };
  const where = productId ? { productId } : {};
  const assemblyLines = await prisma.assemblyLine.findMany({ where: where as any, orderBy: { name: 'asc' } });
  res.json(assemblyLines);
};

export const getAssemblyLine = async (req: Request, res: Response) => {
  const { id } = req.params;
  const assemblyLine = await prisma.assemblyLine.findUnique({ where: { id } });
  if (!assemblyLine) return res.status(404).json({ message: 'AssemblyLine not found' });
  res.json(assemblyLine);
};

export const createAssemblyLine = async (req: Request, res: Response) => {
  const { name, active = true, productId } = req.body;
  if (!name) return res.status(400).json({ message: 'Name is required' });
  try {
    const assemblyLine = await prisma.assemblyLine.create({ data: { name, active, productId } });
    res.status(201).json(assemblyLine);
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};

export const updateAssemblyLine = async (req: Request, res: Response) => {
  const { id } = req.params;
  const { name, active, productId } = req.body;
  try {
    const assemblyLine = await prisma.assemblyLine.update({ where: { id }, data: { name, active, productId } });
    res.json(assemblyLine);
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};

export const deleteAssemblyLine = async (req: Request, res: Response) => {
  const { id } = req.params;
  try {
    await prisma.assemblyLine.delete({ where: { id } });
    res.status(204).send();
  } catch (err: any) {
    res.status(400).json({ message: err.message });
  }
};
