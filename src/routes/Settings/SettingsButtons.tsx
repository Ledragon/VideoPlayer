import { SpeedDial, SpeedDialAction } from '@mui/material';

import AddIcon from '@mui/icons-material/Add';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import UpdateIcon from '@mui/icons-material/Update';
import CleaningServicesIcon from '@mui/icons-material/CleaningServices';

import {
  useCleanVideosMutation,
  useLoadVideosMutation
} from '../../redux/settings.api';

interface PropType {
  onAddDirectory: () => void;
}

const SettingsButtons = (props: PropType) => {
  const [loadVideos, loadVideosStatus] = useLoadVideosMutation();
  const [cleanVideos, cleanVideosStatus] = useCleanVideosMutation();

  const handleLoadVideos = () => {
    loadVideos();
  };

  const handleCleanVideos = () => {
    cleanVideos();
  };

  const handleAddDirectory = () => {
    props.onAddDirectory();
  };

  return (
    <SpeedDial
      icon={<MoreVertIcon />}
      ariaLabel=""
      direction="left"
      sx={{
        position: 'absolute',
        right: (theme) => theme.spacing(2),
        bottom: (theme) => theme.spacing(2)
      }}
      FabProps={{ size: 'small' }}
    >
      <SpeedDialAction
        icon={<UpdateIcon />}
        tooltipTitle="Load new videos"
        onClick={handleLoadVideos}
      ></SpeedDialAction>
      <SpeedDialAction
        icon={<CleaningServicesIcon />}
        tooltipTitle="Clean deleted videos"
        onClick={handleCleanVideos}
      ></SpeedDialAction>
      <SpeedDialAction
        icon={<AddIcon />}
        tooltipTitle="Add new directory"
        onClick={handleAddDirectory}
      ></SpeedDialAction>
    </SpeedDial>
  );
};

export default SettingsButtons;
