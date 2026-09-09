import { Router } from 'express';
import * as controller from '../controllers/allocationController';
import { authenticate } from '../middleware/authMiddleware';

const router = Router({ mergeParams: true });

router.get('/', controller.listAllocations);
router.post('/', authenticate, controller.createAllocations);
router.put('/reorder', authenticate, controller.reorderAllocations);
router.delete('/:allocationId', authenticate, controller.deleteAllocation);

export default router;
