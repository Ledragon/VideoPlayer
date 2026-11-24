import { Box, Paper } from '@mui/material';
import Playlist from '../Videos/Playlist';

interface PropType {}

const PlaylistPage = (props: PropType) => {
  const {} = props;
  return (
    <Box p={1}>
      <Paper>
        <Playlist></Playlist>
      </Paper>
    </Box>
  );
};

export default PlaylistPage;
