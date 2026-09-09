import { listProducts, createProduct, getProduct } from '../src/controllers/productController';
import prisma from '../src/prismaClient';

jest.mock('../src/prismaClient', () => ({
  product: {
    findMany: jest.fn(),
    findUnique: jest.fn(),
    create: jest.fn(),
    update: jest.fn(),
    delete: jest.fn()
  }
}));

const buildRes = () => {
  const res: any = {};
  res.json = jest.fn();
  res.status = jest.fn().mockReturnValue(res);
  res.send = jest.fn();
  return res;
};

describe('productController', () => {
  afterEach(() => jest.clearAllMocks());

  test('listProducts returns products', async () => {
    (prisma.product.findMany as jest.Mock).mockResolvedValue([{ id: '1', name: 'P1' }]);
    const req: any = {};
    const res = buildRes();

    await listProducts(req, res);

    expect(prisma.product.findMany).toHaveBeenCalled();
    expect(res.json).toHaveBeenCalledWith([{ id: '1', name: 'P1' }]);
  });

  test('getProduct returns 404 when not found', async () => {
    (prisma.product.findUnique as jest.Mock).mockResolvedValue(null);
    const req: any = { params: { id: 'missing' } };
    const res = buildRes();

    await getProduct(req, res);

    expect(res.status).toHaveBeenCalledWith(404);
    expect(res.json).toHaveBeenCalledWith({ message: 'Product not found' });
  });

  test('createProduct creates product', async () => {
    const newProd = { id: '2', name: 'New' };
    (prisma.product.create as jest.Mock).mockResolvedValue(newProd);
    const req: any = { body: { name: 'New' } };
    const res = buildRes();

    await createProduct(req, res);

    expect(prisma.product.create).toHaveBeenCalledWith({ data: { name: 'New' } });
    expect(res.status).toHaveBeenCalledWith(201);
    expect(res.json).toHaveBeenCalledWith(newProd);
  });
});
