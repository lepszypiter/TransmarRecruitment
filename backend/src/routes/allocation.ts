import { Router } from 'express';
import * as controller from '../controllers/allocationController';

const router = Router({ mergeParams: true });

router.get('/', controller.listAllocations);
router.post('/', controller.createAllocations);
router.put('/reorder', controller.reorderAllocations);
router.delete('/:allocationId', controller.deleteAllocation);

export default router;
